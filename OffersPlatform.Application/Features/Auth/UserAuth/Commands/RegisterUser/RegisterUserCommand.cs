using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.UserAuth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<AuthDto?>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
}