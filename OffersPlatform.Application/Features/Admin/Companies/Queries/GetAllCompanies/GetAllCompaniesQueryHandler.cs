using System.Net;
using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllCompanies;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto?>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<CompanyDto?>> Handle(GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllCompaniesAsync(cancellationToken);
 
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }
}