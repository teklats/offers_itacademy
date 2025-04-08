using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

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

    var company = await _companyRepository.
        GetCompanyByEmailAsync(request.Email, cancellationToken)
        .ConfigureAwait(false);

    if (company is null)
    {
      throw new NotFoundException("Company Not Found.");
    }

    var companyPasswordHash = await _companyRepository.
        GetPasswordHashAsync(request.Email, cancellationToken)
        .ConfigureAwait(false);

    bool isPasswordValid = _passwordHasher.VerifyPassword(companyPasswordHash, request.Password);
    if (!isPasswordValid)
    {
      throw new BadRequestException("Invalid password.");
    }

    var token = _authService.Login(company.Role, company.Id);

    return new AuthDto
    {
      Id = company.Id,
      Name = company.Name,
      Email = company.Email,
      Token = token
    };
  }
}
