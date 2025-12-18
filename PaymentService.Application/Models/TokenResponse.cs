namespace PaymentService.Application.Models;

public record TokenResponse(bool IsSuccess, string GatewayUrl, Guid Token, string Message = "توکن با موفقیت ایجاد شد")
{ }