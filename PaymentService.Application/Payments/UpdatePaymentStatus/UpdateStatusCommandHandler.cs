using MediatR;
using PaymentService.Application.Models;
using PaymentService.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using MassTransit;
using SHARE.Model;

namespace PaymentService.Application.Payments.UpdatePaymentStatus;

public class UpdateStatusCommandHandler(ITransactionRepository _repository, IPublishEndpoint publisher)
    : IRequestHandler<UpdateStatusCommand, UpdateStatusResponse>
{
    public async Task<UpdateStatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _repository.GetByToken(token: request.Token);
        if (transaction is null)
            throw new ValidationException(message: "توکن نامعتبر است");

        transaction.Status = request.IsSuccess switch
        {
            true => PaymentStatus.Success,
            _ => PaymentStatus.Failed
        };

        transaction.RRN = request.IsSuccess ? request.Rrn : null;
        transaction.UpdatedAt = DateTime.Now;
        await _repository.SaveChanges();

        await publisher.Publish(new UpdateStatusMessage(request.Token, request.IsSuccess, request.Rrn), cancellationToken);

        return new(IsSuccess: true, Message: "وضعیت با موفقیت به روزرسانی شد");

    }
}