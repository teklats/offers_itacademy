using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
namespace OffersPlatform.Application.Features.Auth.UserAuth.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthDto?>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _authService = authService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new BadRequestException("Email and password must be provided.");

        var user = await _userRepository.
            GetActiveUserByEmailAsync(request.Email, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
        {
            throw new NotFoundException("User Not Found.");
        }

        var userPasswordHash = await _userRepository.
            GetPasswordHashAsync(request.Email, cancellationToken)
            .ConfigureAwait(false);

        bool isPasswordValid = _passwordHasher.VerifyPassword(userPasswordHash, request.Password);
        if (!isPasswordValid)
        {
            throw new BadRequestException("Invalid password.");
        }

        var token = _authService.Login(user.Role, user.Id);

        return new AuthDto
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Token = token
        };
    }
}
