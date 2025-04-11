using FluentValidation;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;

public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(200).WithMessage("Offer name must be less than 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Offer description is required.")
            .MaximumLength(1000).WithMessage("Offer description must be less than 1000 characters.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");

        RuleFor(x => x.InitialQuantity)
            .GreaterThan(0).WithMessage("Initial quantity must be greater than 0.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.Now).WithMessage("Expiration date must be in the future.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company ID is required.");
    }
}
