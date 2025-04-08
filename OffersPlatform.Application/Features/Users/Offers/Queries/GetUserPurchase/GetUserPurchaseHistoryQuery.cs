using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Users.Offers.Queries.GetUserPurchase;

public class GetUserPurchaseHistoryQuery : IRequest<IEnumerable<PurchaseDto>>
{
    public Guid UserId { get; set; }

    public GetUserPurchaseHistoryQuery(Guid userId)
    {
        UserId = userId;
    }
}