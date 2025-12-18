using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Application.Common;
using PaymentService.Application.Models;
using PaymentService.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Payments.UpdatePaymentStatus;

public class UpdateStatusCommandHandler(ITransactionRepository _repository)
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

        await _repository.SaveChanges();
        return new(IsSuccess: true, Message: "وضعیت با موفقیت به روزرسانی شد");

    }
}