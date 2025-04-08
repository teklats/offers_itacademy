using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Offers.Queries.GetPreferredActiveOffers;

public class GetPreferredActiveOffersQuery : IRequest<IEnumerable<OfferDto>>
{
    public Guid UserId { get; set; }

    public GetPreferredActiveOffersQuery(Guid userId)
    {
        UserId = userId;
    }
}