using MediatR;
using PaymentService.Application.Models;

namespace PaymentService.Application.Payments.UpdatePaymentStatus;

public record UpdateStatusCommand(Guid Token, bool IsSuccess, string? Rrn) : IRequest<UpdateStatusResponse>
{
}