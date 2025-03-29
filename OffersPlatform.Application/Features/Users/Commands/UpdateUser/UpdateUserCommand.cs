using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<UserDto>
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}