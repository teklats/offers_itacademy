using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;

public class LoginCompanyCommand : IRequest<AuthDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}