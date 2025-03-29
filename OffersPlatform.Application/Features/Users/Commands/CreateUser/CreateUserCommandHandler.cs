using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Users.Commands.CreateUser;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler 
    : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddUserAsync(user, cancellationToken);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}