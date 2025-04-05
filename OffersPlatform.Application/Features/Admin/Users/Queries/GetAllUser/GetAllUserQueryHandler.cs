using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Users.Queries.GetAllUser;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserDto?>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<IEnumerable<UserDto?>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users = _userRepository.GetAllActiveUsersAsync(cancellationToken);
        if (users == null)
        {
            throw new Exception("Users not found");
        }

        return users;
    }
}