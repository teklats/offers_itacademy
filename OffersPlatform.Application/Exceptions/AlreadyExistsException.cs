using System.Net;

namespace OffersPlatform.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public string Code = "ALREADY_EXISTS";

    public AlreadyExistsException(string message) : base(message){}
    
}