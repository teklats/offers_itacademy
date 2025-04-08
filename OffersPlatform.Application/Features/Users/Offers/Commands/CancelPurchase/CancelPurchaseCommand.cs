using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;

public class CancelPurchaseCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid PurchaseId { get; set; }

    public CancelPurchaseCommand(Guid userId, Guid purchaseId)
    {
        UserId = userId;
        PurchaseId = purchaseId;
    }
}