namespace GatewayService.Application.Models;

public record PayResponse(bool IsSuccess, Guid Token, string? Rrn, decimal? Amount, string? Message, string? RedirectUrl)
{
}