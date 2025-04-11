// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthDto?>
    {
        private readonly IAuthService _authService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IAuthService authService, ICompanyRepository companyRepository,
            IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _authService = authService;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var company = await _companyRepository
                .GetCompanyByEmailAsync(request.Email, cancellationToken)
                .ConfigureAwait(false);

            if (company is not null)
                return await LoginCompanyAsync(company, request.Password, cancellationToken).ConfigureAwait(false);

            var user = await _userRepository
                .GetActiveUserByEmailAsync(request.Email, cancellationToken)
                .ConfigureAwait(false);

            if (user is not null)
                return await LoginUserAsync(user, request.Password, cancellationToken).ConfigureAwait(false);

            throw new NotFoundException("Account not found.");
        }

        private static void ValidateRequest(LoginCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new BadRequestException("Email and password must be provided.");
        }

        private async Task<AuthDto?> LoginCompanyAsync(Company company, string? password, CancellationToken cancellationToken)
        {
            var passwordHash = await _companyRepository
                .GetPasswordHashAsync(company.Email, cancellationToken)
                .ConfigureAwait(false);

            if (!_passwordHasher.VerifyPassword(passwordHash, password))
                throw new BadRequestException("Invalid password.");

            var token = _authService.Login(company.Role, company.Id);

            return new AuthDto
            {
                Id = company.Id,
                Name = company.Name,
                Email = company.Email,
                Role = company.Role,
                Token = token
            };
        }

        private async Task<AuthDto?> LoginUserAsync(User user, string? password, CancellationToken cancellationToken)
        {
            var passwordHash = await _userRepository
                .GetPasswordHashAsync(user.Email, cancellationToken)
                .ConfigureAwait(false);

            if (!_passwordHasher.VerifyPassword(passwordHash, password))
                throw new BadRequestException("Invalid password.");

            var token = _authService.Login(user.Role, user.Id);

            return new AuthDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}
