namespace OffersPlatform.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public readonly string Code = "UNAUTHORIZED";
    public UnauthorizedException(string message) : base(message){}
}
