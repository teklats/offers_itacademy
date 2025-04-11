// Copyright (C) TBC Bank. All Rights Reserved.

using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Domain.Enums;

using Moq;
using Xunit;
using FluentAssertions;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Auth.Commands.Login;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.UnitTests.AuthTest;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _handler = new LoginCommandHandler(
            _mockAuthService.Object,
            _mockCompanyRepository.Object,
            _mockUserRepository.Object,
            _mockPasswordHasher.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Return_AuthDto_For_Valid_User_Login()
    {
        // Arrange
        var request = new LoginCommand { Email = "user@example.com", Password = "password123" };
        var user = new User { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "John", Role = UserRole.Customer };

        _mockUserRepository
            .Setup(repo => repo.GetActiveUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(repo => repo.GetPasswordHashAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync("hashedPassword");

        _mockPasswordHasher
            .Setup(ph => ph.VerifyPassword("hashedPassword", request.Password))
            .Returns(true);

        _mockAuthService
            .Setup(auth => auth.Login(user.Role, user.Id))
            .Returns("generated-token");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(user.Id);
        result?.Name.Should().Be(user.UserName);
        result?.Role.Should().Be(user.Role);
        result?.Token.Should().Be("generated-token");

        _mockUserRepository.Verify(repo => repo.GetActiveUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()), Times.Once);
        _mockPasswordHasher.Verify(ph => ph.VerifyPassword("hashedPassword", request.Password), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_And_Company_Are_Not_Found()
    {
        // Arrange
        var request = new LoginCommand { Email = "nonexistent@example.com", Password = "password123" };

        _mockUserRepository
            .Setup(repo => repo.GetActiveUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _mockCompanyRepository
            .Setup(repo => repo.GetCompanyByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_BadRequestException_When_Password_Is_Incorrect()
    {
        // Arrange
        var request = new LoginCommand { Email = "user@example.com", Password = "wrongPassword" };
        var user = new User { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "John", Role = UserRole.Customer };

        _mockUserRepository
            .Setup(repo => repo.GetActiveUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(repo => repo.GetPasswordHashAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync("hashedPassword");

        _mockPasswordHasher
            .Setup(ph => ph.VerifyPassword("hashedPassword", request.Password))
            .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
