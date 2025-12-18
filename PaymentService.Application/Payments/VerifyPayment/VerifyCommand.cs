using MediatR;
using PaymentService.Application.Models;

namespace PaymentService.Application.Payments.VerifyPayment;

public record VerifyCommand(Guid Token, string AppCode) : IRequest<VerifyResponse>
{
}