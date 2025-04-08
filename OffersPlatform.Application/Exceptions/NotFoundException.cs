
namespace OffersPlatform.Application.Exceptions;

public class NotFoundException : Exception
{
    public readonly string Code = "NOT_FOUND";

    public NotFoundException(string message) : base(message){}
    
}