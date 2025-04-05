using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllCompanies;

public class GetAllCompaniesQuery : IRequest<IEnumerable<CompanyDto>>
{
    
}