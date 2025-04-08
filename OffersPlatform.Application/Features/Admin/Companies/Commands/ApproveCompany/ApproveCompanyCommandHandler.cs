using System.Net;
using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Enums;


namespace OffersPlatform.Application.Features.Admin.Companies.Commands.ApproveCompany;

public class ApproveCompanyCommandHandler : IRequestHandler<ApproveCompanyCommand, bool>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public ApproveCompanyCommandHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ApproveCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(request.Id, cancellationToken);

        if (company is null)
        {
            throw new NotFoundException("Company Not Found");
        }

        if (company.Status == CompanyStatus.Active)
        {
            return false;
        }
        company.Status = request.Status;
        await _companyRepository.UpdateAsync(company, cancellationToken);
        await _companyRepository.SaveAsync(cancellationToken);

        return true;
    }
}