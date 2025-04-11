using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Enums;


namespace OffersPlatform.Application.Features.Admin.Companies.Commands.ApproveCompany;

public class ApproveCompanyCommandHandler : IRequestHandler<ApproveCompanyCommand, bool>
{
    private readonly ICompanyRepository _companyRepository;

    public ApproveCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> Handle(ApproveCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository
            .GetCompanyByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        Console.WriteLine("Requested ID: " + request.Id);

        if (company is null)
        {
            throw new NotFoundException("Company Not Found");
        }

        if (company.Status == CompanyStatus.Active)
        {
            return false;
        }
        company.Status = request.Status;
        await _companyRepository.UpdateAsync(company, cancellationToken).ConfigureAwait(false);
        await _companyRepository
            .SaveAsync(cancellationToken)
            .ConfigureAwait(false);

        return true;
    }
}
