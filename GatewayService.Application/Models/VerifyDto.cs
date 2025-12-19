namespace GatewayService.Application.Models;

public class VerifyDto
{
    public required Guid Token { get; set; }
    public required string AppCode { get; set; }
}