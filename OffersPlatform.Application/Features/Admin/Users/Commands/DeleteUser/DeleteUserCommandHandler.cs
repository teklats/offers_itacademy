using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Features.Admin.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand message, CancellationToken cancellationToken)
    {
        await _userRepository.SoftDeleteAsync(message.Id, cancellationToken);
        return Unit.Value;
    }
}