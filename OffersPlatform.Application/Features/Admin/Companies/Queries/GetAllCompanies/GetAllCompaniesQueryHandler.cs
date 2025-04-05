using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllCompanies;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto?>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    
    public async Task<IEnumerable<CompanyDto?>> Handle(GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllCompaniesAsync(cancellationToken);
        if (companies == null)
        {
            throw new Exception("Company not found");
        }
        return companies;
    }
}