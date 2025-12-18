namespace PaymentService.Application.Models;

public class VerifyResponse
{
    public bool IsSuccess { get; set; }
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public string? Rrn { get; set; }
    public string? ReservationNumber { get; set; }
    public string Message { get; set; } = null!;
}