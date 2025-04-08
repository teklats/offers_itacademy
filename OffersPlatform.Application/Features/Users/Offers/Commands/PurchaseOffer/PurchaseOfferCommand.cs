using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Offers.Commands.PurchaseOffer;

public class PurchaseOfferCommand : IRequest<PurchaseDto>
{
    public Guid UserId { get; set; }
    public Guid OfferId { get; set; }
    public int Quantity { get; set; }

    public PurchaseOfferCommand(Guid userId, Guid offerId, int quantity)
    {
        UserId = userId;
        OfferId = offerId;
        Quantity = quantity;
    }
    
}