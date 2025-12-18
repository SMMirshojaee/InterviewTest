using System.ComponentModel.DataAnnotations;

namespace PaymentService.Api.Models;

public class TokenRequest
{
    public required string TerminalNo { get; set; }
    [Range(1, double.MaxValue)]
    public required decimal Amount { get; set; }
    public required string RedirectUrl { get; set; }
    public required string ReservationNumber { get; set; }
    public required string PhoneNumber { get; set; }
}