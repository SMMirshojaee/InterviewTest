using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Entities;

[Index(nameof(Token), IsUnique = true)]
public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public  string TerminalNo { get; set; } = null!;

    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string RedirectUrl { get; set; } = null!;
    
    [Required]
    public  string ReservationNumber { get; set; } = null!;
    
    [Required]
    public  string PhoneNumber { get; set; } = null!;

    public string Token { get; set; } // unique GUID

    public string? RRN { get; set; } // nullable

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? AppCode { get; set; } // nullable
}