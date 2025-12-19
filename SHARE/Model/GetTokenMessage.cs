namespace SHARE.Model;

public record GetTokenMessage(Guid Token,string PhoneNumber,decimal Amount,string RedirectUrl)
{
}