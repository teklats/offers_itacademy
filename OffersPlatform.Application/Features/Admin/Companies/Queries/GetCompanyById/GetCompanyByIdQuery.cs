using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQuery : IRequest<CompanyDto>
{
    public Guid Id { get; set; }

    public GetCompanyByIdQuery(Guid id)
    {
        Id = id;
    }
}