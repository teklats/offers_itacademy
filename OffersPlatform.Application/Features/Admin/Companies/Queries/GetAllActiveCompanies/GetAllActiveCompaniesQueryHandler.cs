
using System.Net;
using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;

public class GetAllActiveCompaniesQueryHandler : IRequestHandler<GetAllActiveCompaniesQuery, IEnumerable<CompanyDto?>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetAllActiveCompaniesQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto?>> Handle(GetAllActiveCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllActiveCompaniesAsync(cancellationToken);
 
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }
}