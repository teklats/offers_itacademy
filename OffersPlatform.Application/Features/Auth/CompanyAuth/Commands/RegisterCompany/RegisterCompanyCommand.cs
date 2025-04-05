using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.RegisterCompany;


public class RegisterCompanyCommand : IRequest<AuthDto>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }
}