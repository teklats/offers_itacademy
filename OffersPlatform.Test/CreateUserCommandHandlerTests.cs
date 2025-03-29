using Moq;
using Xunit;
using OffersPlatform.Application.Features.Users.Commands.CreateUser;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Test;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateUser_And_ReturnUserDto()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "johndoe@example.com",
            Password = "Test1234",
            Role = UserRole.Customer
        };

        _userRepositoryMock
            .Setup(repo => repo.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.FirstName, result.FirstName);
        Assert.Equal(command.LastName, result.LastName);
        Assert.Equal(command.Email, result.Email);
        Assert.NotEqual(default(Guid), result.Id);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}