using System.ComponentModel.DataAnnotations;

namespace GatewayService.Application.Models;

public class UpdateStatusDto
{
    [Required(ErrorMessage = "توکن الزامی است")]
    public Guid Token { get; set; }
    [Required(ErrorMessage = "وضعیت تراکنش الزامی است")]
    public bool IsSuccess { get; set; }
    public string? Rrn { get; set; }
}