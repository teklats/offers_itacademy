using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;

public class UpdateUserBalanceCommandHandler : IRequestHandler<UpdateUserBalanceCommand, UserBalanceDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserBalanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserBalanceDto?> Handle(UpdateUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository
            .GetActiveUserByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
            throw new NotFoundException("User not found");

        user.Balance += request.Balance;

        await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken)
            .ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken)
            .ConfigureAwait(false);

        return _mapper.Map<UserBalanceDto>(user);
    }
}
