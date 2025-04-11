using AutoMapper;
using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterUser;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using Xunit;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _authServiceMock = new Mock<IAuthService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _mapperMock = new Mock<IMapper>();

        _handler = new RegisterUserCommandHandler(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _authServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistsException_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "existing@example.com",
            Password = "123456"
        };

        _unitOfWorkMock.Setup(x =>
                x.UserRepository.GetActiveUserByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User());

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    [Fact]
    public async Task Handle_ShouldRegisterUserAndReturnAuthDto_WhenValid()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Password = "Password123",
            PhoneNumber = "1234567890"
        };

        var userEntity = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Username,
            Email = command.Email,
            Role = UserRole.Customer
        };

        _unitOfWorkMock.Setup(x =>
                x.UserRepository.GetActiveUserByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _passwordHasherMock.Setup(x => x.HashPassword(command.Password)).Returns("hashedPassword");

        _mapperMock.Setup(x => x.Map<User>(command)).Returns(userEntity);

        _unitOfWorkMock.Setup(x =>
                x.UserRepository.AddAsync(userEntity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x =>
                x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _authServiceMock.Setup(x =>
                x.RegisterUser(userEntity, "hashedPassword"))
            .Returns("generated-jwt-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("generated-jwt-token");
        result.Email.Should().Be(command.Email);
        result.Name.Should().Be(command.Username);
        result.Role.Should().Be(UserRole.Customer);
    }
}
