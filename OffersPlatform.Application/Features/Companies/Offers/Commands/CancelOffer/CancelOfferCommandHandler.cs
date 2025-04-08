using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;

public class CancelOfferCommandHandler : IRequestHandler<CancelOfferCommand, bool>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository; // assuming this gives access to user balances
    private readonly IPurchaseRepository _purchaseRepository;

    public CancelOfferCommandHandler(
        IOfferRepository offerRepository,
        IUserRepository userRepository,
        IPurchaseRepository purchaseRepository)
    {
        _offerRepository = offerRepository;
        _userRepository = userRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<bool> Handle(CancelOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository
            .GetByIdAsync(request.OfferId, request.CompanyId, cancellationToken)
            .ConfigureAwait(false);
        if (offer == null)
            return false;

        var canCancel = DateTime.UtcNow - offer.CreatedAt <= TimeSpan.FromMinutes(10);
        if (!canCancel)
            return false;

        var purchases = await _purchaseRepository
            .GetByOfferIdAsync(request.OfferId, cancellationToken)
            .ConfigureAwait(false);


        foreach (var purchase in purchases)
        {
            // Refund the buyer
            var buyers = await _purchaseRepository
                .GetUserByPurchaseId(purchase.Id, cancellationToken)
                .ConfigureAwait(false);
            foreach (var buyerId in buyers)
            {
                var buyer = await _userRepository
                    .GetActiveUserByIdAsync(buyerId, cancellationToken)
                    .ConfigureAwait(false);
                if (buyer != null)
                {
                    buyer.Balance += purchase.TotalPrice;
                    await _userRepository
                        .UpdateAsync(buyer, cancellationToken)
                        .ConfigureAwait(false);
                }
            }

        }

        var result = await _offerRepository
            .CancelOfferAsync(request.OfferId, request.CompanyId, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }
}
