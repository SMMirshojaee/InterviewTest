using GatewayService.Application.Models;
using MediatR;

namespace GatewayService.Application.Features.Pay;

public record PayCommand(Guid Token) : IRequest<PayResponse>
{
}