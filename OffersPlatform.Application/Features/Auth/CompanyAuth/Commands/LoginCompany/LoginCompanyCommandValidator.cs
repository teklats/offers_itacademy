using FluentValidation;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;


public class LoginCompanyCommandValidator : AbstractValidator<LoginCompanyCommand>
{
    public LoginCompanyCommandValidator()
    {
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}