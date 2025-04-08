using System.Net;
using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Admin.Users.Queries.GetAllUser;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserDto?>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public GetAllUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<UserDto?>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetAllActiveUsersAsync(cancellationToken)
            .ConfigureAwait(false);

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}
