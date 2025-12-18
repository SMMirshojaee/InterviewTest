namespace PaymentService.Application.Models;

public record TokenResponse
{
    public bool IsSuccess { get; set; }//": true,
    public string GatewayUrl { get; set; }//": "https://localhost:5002/api/gateway/pay/{token}",
    public Guid Token { get; set; }//": "guid",
    public string Message { get; set; } = "توکن با موفقیت ایجاد شد";
}