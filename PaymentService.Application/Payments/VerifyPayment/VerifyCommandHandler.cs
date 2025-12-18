using MediatR;
using PaymentService.Application.Models;
using PaymentService.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Payments.VerifyPayment;

public class VerifyCommandHandler(ITransactionRepository repository) : IRequestHandler<VerifyCommand, VerifyResponse>
{
    public async Task<VerifyResponse> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await repository.GetByToken(request.Token);
        if (transaction is null)
            throw new ValidationException("توکن نامعتبر است");

        if (IsExpired(transaction))
            return new VerifyResponse
            (
                IsSuccess: false,
                Status: nameof(PaymentStatus.Expired),
                Amount: transaction.Amount,
                ReservationNumber: transaction.ReservationNumber,
                Rrn: null,
                Message: "زمان پرداخت منقضی شده است"
            );
        if (transaction.Status == PaymentStatus.Failed)
            return new VerifyResponse
            (
                IsSuccess: false,
                Status: nameof(PaymentStatus.Failed),
                Amount: transaction.Amount,
                ReservationNumber: transaction.ReservationNumber,
                Rrn: null,
                Message: "پرداخت ناموفق بود"
            );
        if (transaction.Status == PaymentStatus.Success)
        {
            await repository.SetAppCode(transaction, request.AppCode);
            return new VerifyResponse
            (
                IsSuccess: true,
                Status: nameof(PaymentStatus.Success),
                Amount: transaction.Amount,
                ReservationNumber: transaction.ReservationNumber,
                Rrn: transaction.RRN,
                Message: "پرداخت با موفقیت تایید شد"
            );
        }

        throw new ValidationException("توکن نامعتبر است");

        static bool IsExpired(Transaction transaction) =>
            transaction.CreatedAt.AddMinutes(2) < DateTime.Now;
    }
}