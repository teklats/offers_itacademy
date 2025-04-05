using MediatR;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Admin.Companies.Commands.ApproveCompany;

public class ApproveCompanyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public CompanyStatus Status { get; set; }

    public ApproveCompanyCommand(Guid id, CompanyStatus status)
    {
        Id = id;
        Status = status;
    }
}