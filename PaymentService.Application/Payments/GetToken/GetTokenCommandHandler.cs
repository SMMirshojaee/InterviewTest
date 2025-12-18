using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Application.Common;
using PaymentService.Application.Models;
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
            return new TokenResponse() with { Message = "ورودی amount نامعتبر است" };
        var token = Guid.NewGuid();
        var newTransaction = new Transaction()
        {
            Amount = request.Amount,
            RedirectUrl = request.RedirectUrl,
            ReservationNumber = request.ReservationNumber,
            PhoneNumber = request.PhoneNumber,
            Token = token.ToString(),
        };

        await _repository.Add(newTransaction);
        return new TokenResponse() with { IsSuccess = true, GatewayUrl = $"{_gatewayServiceUrl}{token}", Token = token };

    }
}