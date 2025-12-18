namespace PaymentService.Api.Models;

public class VerifyRequest
{
    public required Guid Token { get; set; }
    public required string AppCode { get; set; }
}