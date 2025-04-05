using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetActiveUserByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new Exception("User Not Found");
        }

        return user;
    }
}