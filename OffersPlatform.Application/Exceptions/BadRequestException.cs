namespace OffersPlatform.Application.Exceptions;

public class BadRequestException : Exception
{
    public string Code = "BAD_REQUEST";

    public BadRequestException(string message) : base(message){}
}
