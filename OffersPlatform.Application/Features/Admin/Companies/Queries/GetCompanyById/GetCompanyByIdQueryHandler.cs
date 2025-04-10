using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Admin.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository
            .GetCompanyByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (company is null)
        {
            throw new NotFoundException("Company Not Found");
        }
        return _mapper.Map<CompanyDto>(company);
    }
}
