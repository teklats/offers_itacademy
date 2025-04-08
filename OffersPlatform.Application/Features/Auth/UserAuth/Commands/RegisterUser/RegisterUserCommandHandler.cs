using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Auth.UserAuth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthDto?>
{
    private readonly IAuthService _authService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IAuthService authService, IPasswordHasher passwordHasher,
        IUserRepository userRepository, IMapper mapper)
    {
        _authService = authService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<AuthDto?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var existingUser = await _userRepository.
            GetActiveUserByEmailAsync(request.Email, cancellationToken)
            .ConfigureAwait(false);

        if (existingUser != null)
        {
            throw new AlreadyExistsException("User Already Exists.");
        }

        var user = _mapper.Map<User>(request);

        user.PasswordHash = hashedPassword;
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;

        await _userRepository.
            AddAsync(user, cancellationToken)
            .ConfigureAwait(false);
        await _userRepository.
            SaveAsync(cancellationToken)
            .ConfigureAwait(false);

        var token = _authService.RegisterUser(user, hashedPassword);

        return new AuthDto
        {
            Token = token,
            Name = user.UserName,
            Id = user.Id,
            Email = user.Email,
        };

    }
}
