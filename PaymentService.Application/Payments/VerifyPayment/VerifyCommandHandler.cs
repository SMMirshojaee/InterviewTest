using MediatR;
using PaymentService.Application.Models;
using PaymentService.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using MassTransit;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using SHARE.Model;

namespace PaymentService.Application.Payments.VerifyPayment;

public class VerifyCommandHandler(ITransactionRepository repository, IPublishEndpoint publisher) : IRequestHandler<VerifyCommand, VerifyResponse>
{
    public async Task<VerifyResponse> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await repository.GetByToken(request.Token);
        if (transaction is null)
            throw new ValidationException("توکن نامعتبر است");

        if (IsExpired(transaction))
        {
            await publisher.Publish(new VerifyMessage(request.Token, nameof(PaymentStatus.Expired)), cancellationToken);
            return new VerifyResponse
            (
                IsSuccess: false,
                Status: nameof(PaymentStatus.Expired),
                Amount: transaction.Amount,
                ReservationNumber: transaction.ReservationNumber,
                Rrn: null,
                RedirectUrl: transaction.RedirectUrl,
                Message: "زمان پرداخت منقضی شده است"
            );
        }
        switch (transaction.Status)
        {
            case PaymentStatus.Failed:
                await publisher.Publish(new VerifyMessage(request.Token, nameof(PaymentStatus.Failed)),
                    cancellationToken);

                return new VerifyResponse
                (
                    IsSuccess: false,
                    Status: nameof(PaymentStatus.Failed),
                    Amount: transaction.Amount,
                    ReservationNumber: transaction.ReservationNumber,
                    Rrn: null,
                    RedirectUrl: transaction.RedirectUrl,
                    Message: "پرداخت ناموفق بود"
                );
            case PaymentStatus.Success:
                await publisher.Publish(new VerifyMessage(request.Token, nameof(PaymentStatus.Success)),
                    cancellationToken);

                return new VerifyResponse
                (
                    IsSuccess: true,
                    Status: nameof(PaymentStatus.Success),
                    Amount: transaction.Amount,
                    ReservationNumber: transaction.ReservationNumber,
                    Rrn: transaction.RRN,
                    RedirectUrl: transaction.RedirectUrl,
                    Message: "پرداخت با موفقیت تایید شد"
                );
            case PaymentStatus.Pending:
                await publisher.Publish(new VerifyMessage(request.Token, nameof(PaymentStatus.Pending)),
                    cancellationToken);

                await repository.SetAppCode(transaction, request.AppCode);
                return new VerifyResponse
                (
                    IsSuccess: true,
                    Status: nameof(PaymentStatus.Pending),
                    Amount: transaction.Amount,
                    ReservationNumber: transaction.ReservationNumber,
                    Rrn: null,
                    RedirectUrl: transaction.RedirectUrl,
                    Message: "در انتظار پرداخت"
                );
            case PaymentStatus.Expired:
            default:
                await publisher.Publish(new VerifyMessage(request.Token, nameof(PaymentStatus.Expired)),
                    cancellationToken);

                return new VerifyResponse
                (
                    IsSuccess: false,
                    Status: nameof(PaymentStatus.Expired),
                    Amount: transaction.Amount,
                    ReservationNumber: transaction.ReservationNumber,
                    Rrn: null,
                    RedirectUrl: transaction.RedirectUrl,
                    Message: "زمان پرداخت منقضی شده است"
                );
        }

        static bool IsExpired(Transaction transaction) =>
            transaction.CreatedAt.AddMinutes(2) < DateTime.Now;
    }
}