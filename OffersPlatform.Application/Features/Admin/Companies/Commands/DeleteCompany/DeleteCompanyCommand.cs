using MediatR;

namespace OffersPlatform.Application.Features.Admin.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IRequest<Unit>
{
    public Guid CompanyId { get; set; }

    public DeleteCompanyCommand(Guid companyId)
    {
        CompanyId = companyId;
    }
}