using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;

public class GetOfferByIdQuery : IRequest<OfferDto>
{
    public Guid CompanyId { get; set; }
    public Guid OfferId { get; set; }

    public GetOfferByIdQuery(Guid companyId, Guid offerId)
    {
        CompanyId = companyId;
        OfferId = offerId;
    }
}