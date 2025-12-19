using System.ComponentModel.DataAnnotations;

namespace PaymentService.Api.Models;

public class TokenDto
{
    [Required(ErrorMessage = "شماره ترمینال الزامی است")]
    public required string TerminalNo { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "مبلغ باید بیش از 0 باشد")]
    [Required(ErrorMessage = "مبلغ الزامی است")]
    public required decimal Amount { get; set; }
    [Required(ErrorMessage = "آدرس بازگشتی الزامی است")]
    [RegularExpression("^(https?):\\/\\/[^\\s/$.?#].[^\\s]*$",ErrorMessage = "آدرس بازگشتی معتبر نیست")]
    public required string RedirectUrl { get; set; }
    [Required(ErrorMessage = "شماره رزرو  الزامی است")]
    public required string ReservationNumber { get; set; }
    [Required(ErrorMessage = "شماره موبایل الزامی است")]
    [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل وارد شده معتبر نیست.")]
    public required string PhoneNumber { get; set; }
}