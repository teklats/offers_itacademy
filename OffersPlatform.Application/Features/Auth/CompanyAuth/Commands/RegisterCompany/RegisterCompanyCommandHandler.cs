using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.RegisterCompany;


public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, AuthDto?>
{
    private readonly IAuthService _authService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICompanyRepository _companyRepository;

    public RegisterCompanyCommandHandler(IAuthService authService, IPasswordHasher passwordHasher,
        ICompanyRepository companyRepository)
    {
        _authService = authService;
        _passwordHasher = passwordHasher;
        _companyRepository = companyRepository;
    }

    public async Task<AuthDto?> Handle(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordHasher.HashPassword(command.Password);
        if (await _companyRepository.
                GetCompanyByEmailAsync(command.Email, cancellationToken)
                .ConfigureAwait(false) is not null)
        {
            throw new AlreadyExistsException("Company Already Exists.");
        }
        var company = new Domain.Entities.Company
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            Description = command.Description,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Status = CompanyStatus.Inactive
        };
        await _companyRepository.AddAsync(company, cancellationToken).ConfigureAwait(false);
        var token = _authService.RegisterCompany(company, hashedPassword);
        return new AuthDto
        {
            Token = token,
            Name = company.Name,
            Id = company.Id,
            Email = company.Email,
        };
    }
}
