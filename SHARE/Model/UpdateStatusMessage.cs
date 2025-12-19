namespace SHARE.Model;

public record UpdateStatusMessage(Guid Token, bool IsSuccess, string? Rrn)
{
}