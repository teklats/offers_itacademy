using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterCompany;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using Xunit;

public class RegisterCompanyCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly RegisterCompanyCommandHandler _handler;

    public RegisterCompanyCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _authServiceMock = new Mock<IAuthService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new RegisterCompanyCommandHandler(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _authServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistsException_WhenCompanyEmailExists()
    {
        // Arrange
        var command = new RegisterCompanyCommand
        {
            Email = "test@company.com",
            Password = "Password123",
            Name = "CompanyName",
            Description = "Description",
            PhoneNumber = "123456789",
            Address = "123 Main St"
        };

        _unitOfWorkMock.Setup(x => x.CompanyRepository
                .GetCompanyByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Company());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    [Fact]
    public async Task Handle_ShouldRegisterCompanySuccessfully_WhenEmailNotExists()
    {
        // Arrange
        var command = new RegisterCompanyCommand
        {
            Email = "new@company.com",
            Password = "Secure123",
            Name = "NewCo",
            Description = "We sell stuff",
            PhoneNumber = "555123456",
            Address = "456 Some St"
        };

        _unitOfWorkMock.Setup(x => x.CompanyRepository
                .GetCompanyByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        _passwordHasherMock.Setup(x => x.HashPassword(command.Password)).Returns("hashed-password");

        _authServiceMock.Setup(x => x.RegisterCompany(It.IsAny<Company>(), "hashed-password"))
            .Returns("test-token");

        _unitOfWorkMock.Setup(x => x.CompanyRepository.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("test-token");
        result.Email.Should().Be(command.Email);
        result.Name.Should().Be(command.Name);
        result.Role.Should().Be(UserRole.Company);
    }
}
