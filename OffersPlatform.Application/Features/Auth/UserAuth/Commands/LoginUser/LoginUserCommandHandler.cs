using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Auth.UserAuth.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthDto?>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<AuthDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
        var user = await _userRepository.GetActiveUserByEmailAsync(request.Email, cancellationToken);
        return new AuthDto
        {
            Id = user.Id,
            Name = user?.UserName,
            Email = user?.Email,
            Token = token
        };
    }
}