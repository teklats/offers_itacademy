using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
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

        if (company == null)
        {
            throw new Exception(message: "Company not found");
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