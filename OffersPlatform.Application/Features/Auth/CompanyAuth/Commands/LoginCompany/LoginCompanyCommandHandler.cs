using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;

public class LoginCompanyCommandHandler : IRequestHandler<LoginCompanyCommand, AuthDto?>
{
  private readonly IAuthService _authService;
  private readonly ICompanyRepository _companyRepository;

  public LoginCompanyCommandHandler(IAuthService authService, ICompanyRepository companyRepository)
  {
    _authService = authService;
    _companyRepository = companyRepository;
  }

  public async Task<AuthDto?> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
  {
    var token = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
    var company = await _companyRepository.GetCompanyByEmailAsync(request.Email, cancellationToken);
    
    return new AuthDto
    {
      Id = company.Id,
      Name = company.Name,
      Email = company.Email,
      Token = token,
    };
  }
}