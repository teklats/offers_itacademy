using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;

public class GetAllActiveCompaniesQuery : IRequest<IEnumerable<CompanyDto>>
{
    
}