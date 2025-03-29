using MediatR;

namespace OffersPlatform.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}