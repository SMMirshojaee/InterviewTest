namespace GatewayService.Application.Models;

public record VerifyResponse(bool IsSuccess,
    string? Status,
    decimal Amount,
    string? ReservationNumber,
    string? Rrn,
    string RedirectUrl,
    string Message = null!)
{
}