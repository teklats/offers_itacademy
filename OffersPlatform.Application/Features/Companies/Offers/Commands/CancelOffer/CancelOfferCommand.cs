using MediatR;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;

public class CancelOfferCommand : IRequest<bool>
{
    public Guid CompanyId { get; set; }
    public Guid OfferId { get; set; }

    public CancelOfferCommand(Guid companyId, Guid offerId)
    {
        CompanyId = companyId;
        OfferId = offerId;
    }
}
