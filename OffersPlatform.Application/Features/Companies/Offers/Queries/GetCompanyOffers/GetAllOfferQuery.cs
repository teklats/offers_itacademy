using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;

public class GetAllOfferQuery : IRequest<IEnumerable<OfferDto>>
{
    public Guid CompanyId { get; set; }

    public GetAllOfferQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
}