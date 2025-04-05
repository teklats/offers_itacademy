using MediatR;
using OffersPlatform.Application.DTOs;
namespace OffersPlatform.Application.Features.Auth.UserAuth.Commands.LoginUser;

public class LoginUserCommand : IRequest<AuthDto?>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
}