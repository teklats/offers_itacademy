using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.Commands.Register.RegisterCompany;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, AuthDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;

    public RegisterCompanyCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _authService = authService;
    }

    public async Task<AuthDto?> Handle(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CompanyRepository
                .GetCompanyByEmailAsync(command.Email, cancellationToken)
                .ConfigureAwait(false) is not null)
        {
            throw new AlreadyExistsException("Company Already Exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(command.Password);

        var company = new Domain.Entities.Company
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            Description = command.Description,
            PasswordHash = hashedPassword,
            PhoneNumber = command.PhoneNumber,
            Address = command.Address,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Role = UserRole.Company,
            Status = CompanyStatus.Inactive
        };

        await _unitOfWork.CompanyRepository.AddAsync(company, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        var token = _authService.RegisterCompany(company, hashedPassword);

        return new AuthDto
        {
            Token = token,
            Name = company.Name,
            Id = company.Id,
            Email = company.Email,
            Role = company.Role,
        };
    }
}
