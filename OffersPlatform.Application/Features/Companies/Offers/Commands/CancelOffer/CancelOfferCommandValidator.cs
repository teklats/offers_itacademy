using FluentValidation;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;

public class CancelOfferCommandValidator : AbstractValidator<CancelOfferCommand>
{
    public CancelOfferCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company ID is required.");

        RuleFor(x => x.OfferId)
            .NotEmpty().WithMessage("Offer ID is required.");
    }
}
