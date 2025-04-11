namespace OffersPlatform.Application.Exceptions;

public class ForbiddenException : Exception
{
    public string Code = "FORBIDDEN";
    public ForbiddenException(string message): base(message){}
}
