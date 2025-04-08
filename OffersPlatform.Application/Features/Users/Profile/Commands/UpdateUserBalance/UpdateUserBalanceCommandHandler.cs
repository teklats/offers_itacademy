using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;

public class UpdateUserBalanceCommandHandler : IRequestHandler<UpdateUserBalanceCommand, UserBalanceDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserBalanceCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserBalanceDto> Handle(UpdateUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .UpdateUserBalanceAsync(request.Id, request.Balance, cancellationToken)
            .ConfigureAwait(false);

        return _mapper.Map<UserBalanceDto>(user);
    }
}
