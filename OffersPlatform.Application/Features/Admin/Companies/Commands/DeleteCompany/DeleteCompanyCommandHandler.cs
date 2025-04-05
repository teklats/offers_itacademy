using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;

namespace OffersPlatform.Application.Features.Admin.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Unit>
{
    private readonly ICompanyRepository _companyRepository;

    public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        await _companyRepository.SoftDeleteAsync(request.CompanyId, cancellationToken);
        return Unit.Value;
    }
}