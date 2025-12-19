namespace GatewayService.Application.Common;

public record ServiceUrls
{
    public string PaymentServiceVerifyUrl { get; set; } = null!;
    public string PaymentServiceUpdateStatusUrl { get; set; } = null!;
}