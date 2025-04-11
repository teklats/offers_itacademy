using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.Commands.Register.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IAuthService authService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<AuthDto?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository
            .GetActiveUserByEmailAsync(request.Email, cancellationToken)
            .ConfigureAwait(false);

        if (existingUser != null)
        {
            throw new AlreadyExistsException("User Already Exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = _mapper.Map<User>(request);
        user.PasswordHash = hashedPassword;
        user.IsActive = true;
        user.CreatedAt = DateTime.Now;
        user.Role = UserRole.Customer;

        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        var token = _authService.RegisterUser(user, hashedPassword);

        return new AuthDto
        {
            Token = token,
            Name = user.UserName,
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
        };
    }
}
