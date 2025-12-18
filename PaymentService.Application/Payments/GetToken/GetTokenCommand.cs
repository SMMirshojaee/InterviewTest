using MediatR;
using PaymentService.Application.Models;

namespace PaymentService.Application.Payments.GetToken;

public record GetTokenCommand(string TerminalNo
, decimal Amount
, string RedirectUrl
, string ReservationNumber
, string PhoneNumber
    ) : IRequest<TokenResponse>
{

}