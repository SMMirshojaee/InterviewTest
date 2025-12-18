using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Application.Common;
using PaymentService.Application.Models;
using PaymentService.Domain.Common;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Application.Payments.GetToken;

public class GetTokenCommandHandler : IRequestHandler<GetTokenCommand, TokenResponse>
{
    private readonly ITransactionRepository _repository;
    private readonly string _gatewayServiceUrl;
    public GetTokenCommandHandler(IOptions<ServiceUrls> serviceUrls, ITransactionRepository repository)
    {
        _repository = repository;
        _gatewayServiceUrl = serviceUrls.Value.GatewayServiceUrl;
    }
    public async Task<TokenResponse> Handle(GetTokenCommand request, CancellationToken cancellationToken)
    {
        if (request is { Amount: <= 0 })
            return new TokenResponse { IsSuccess = false, Message = "ورودی amount نامعتبر است" };
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

        await _repository.Add(newTransaction);
        return new TokenResponse { IsSuccess = true, GatewayUrl = $"{_gatewayServiceUrl}{token}", Token = token };
    }
}