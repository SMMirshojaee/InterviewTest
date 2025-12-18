using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Application.Common;
using PaymentService.Application.Models;
using PaymentService.Domain.Common;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Application.Payments.GetToken;

public class GetTokenCommandHandler(IOptions<ServiceUrls> serviceUrls, ITransactionRepository repository)
    : IRequestHandler<GetTokenCommand, TokenResponse>
{
    private readonly string _gatewayServiceUrl = serviceUrls.Value.GatewayServiceUrl;

    public async Task<TokenResponse> Handle(GetTokenCommand request, CancellationToken cancellationToken)
    {
        Guid token = Guid.NewGuid();
        Transaction newTransaction = new()
        {
            Amount = request.Amount,
            TerminalNo = request.TerminalNo,
            RedirectUrl = request.RedirectUrl,
            ReservationNumber = request.ReservationNumber,
            PhoneNumber = request.PhoneNumber,
            Token = token.ToString(),
        };

        await repository.Add(newTransaction);
        return new TokenResponse(IsSuccess: true, GatewayUrl: $"{_gatewayServiceUrl}{token}", Token: token);
    }
}