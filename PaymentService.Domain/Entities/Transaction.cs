using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Entities;

[Index(nameof(Token), IsUnique = true)]
public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(64)]

    public string TerminalNo { get; set; } = null!;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(2048)]

    public string RedirectUrl { get; set; } = null!;

    [Required]
    [MaxLength(256)]

    public string ReservationNumber { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [MaxLength(1024)]
    public string Token { get; set; } = null!;

    [MaxLength(512)]
    public string? RRN { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    [MaxLength(512)]
    public string? AppCode { get; set; }
}