
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;

public class GetAllActiveCompaniesQueryHandler : IRequestHandler<GetAllActiveCompaniesQuery, IEnumerable<CompanyDto?>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllActiveCompaniesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<IEnumerable<CompanyDto?>> Handle(GetAllActiveCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllActiveCompaniesAsync(cancellationToken);
        if (companies == null)
        {
            throw new Exception("Company not found");
        }
        return companies;
    }
}