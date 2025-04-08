using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;

public class LoginCompanyCommandHandler : IRequestHandler<LoginCompanyCommand, AuthDto?>
{
  private readonly IAuthService _authService;
  private readonly ICompanyRepository _companyRepository;
  private readonly IPasswordHasher _passwordHasher;

  public LoginCompanyCommandHandler(IAuthService authService, ICompanyRepository companyRepository, IPasswordHasher passwordHasher)
  {
    _authService = authService;
    _companyRepository = companyRepository;
    _passwordHasher = passwordHasher;
  }

  public async Task<AuthDto?> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
      throw new BadRequestException("Email and password must be provided.");
        
    var company = await _companyRepository.GetCompanyByEmailAsync(request.Email, cancellationToken);
        
    if (company is null)
    {
      throw new NotFoundException("Company Not Found.");
    }
        
    var companyPasswordHash = await _companyRepository.GetPasswordHashAsync(request.Email, cancellationToken);
        
    bool isPasswordValid = _passwordHasher.VerifyPassword(companyPasswordHash, request.Password);
    if (!isPasswordValid)
    {
      throw new BadRequestException("Invalid password.");
    }
        
    var token = await _authService.LoginAsync(company.Role, company.Id, cancellationToken);
        
    return new AuthDto
    {
      Id = company.Id,
      Name = company?.Name,
      Email = company?.Email,
      Token = token
    };
  }
}