using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;

public class UpdateUserBalanceCommand : IRequest<UserBalanceDto?>
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }

    public UpdateUserBalanceCommand(Guid id, decimal balance)
    {
        Id = id;
        Balance = balance;
    }
}
