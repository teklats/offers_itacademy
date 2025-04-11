// // Copyright (C) TBC Bank. All Rights Reserved.
//
// using AutoMapper;
// using FluentAssertions;
// using Moq;
// using OffersPlatform.Application.Common.Interfaces.IRepositories;
// using OffersPlatform.Application.DTOs;
// using OffersPlatform.Application.Exceptions;
// using OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;
// using OffersPlatform.Domain.Entities;
// using Xunit;
//
// namespace OffersPlatform.Application.UnitTests.UserTest;
//
// public class UpdateUserBalanceCommandHandlerTests
// {
//     private readonly Mock<IUserRepository> _userRepositoryMock;
//     private readonly Mock<IMapper> _mapperMock;
//
//     public UpdateUserBalanceCommandHandlerTests()
//     {
//         _userRepositoryMock = new Mock<IUserRepository>();
//         _mapperMock = new Mock<IMapper>();
//     }
//
//     [Fact]
//     public async Task UpdateUserBalanceCommandHandler_ShouldReturnUserBalance_WhenUserIsFound()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var userBalance = 100m;
//         var user = new User { Id = userId, Balance = userBalance };
//         var userBalanceDto = new UserBalanceDto { Id = userId, Balance = userBalance };
//
//         _userRepositoryMock
//             .Setup(x => x.UpdateUserBalanceAsync(userId, userBalance, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(user);
//
//         _mapperMock
//             .Setup(m => m.Map<UserBalanceDto>(user))
//             .Returns(userBalanceDto);
//
//         var handler = new UpdateUserBalanceCommandHandler(_userRepositoryMock.Object, _mapperMock.Object);
//         var command = new UpdateUserBalanceCommand(userId, userBalance);
//
//         // Act
//         var result = await handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().BeEquivalentTo(userBalanceDto);
//         _userRepositoryMock.Verify(x => x.UpdateUserBalanceAsync(userId, userBalance, It.IsAny<CancellationToken>()), Times.Once);
//     }
//
//     [Fact]
//     public async Task UpdateUserBalanceCommandHandler_ShouldThrowNotFoundException_WhenUserNotFound()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var userBalance = 100m;
//
//         _userRepositoryMock
//             .Setup(x => x.UpdateUserBalanceAsync(userId, userBalance, It.IsAny<CancellationToken>()))
//             .ReturnsAsync((User?)null);
//
//         var handler = new UpdateUserBalanceCommandHandler(_userRepositoryMock.Object, _mapperMock.Object);
//         var command = new UpdateUserBalanceCommand(userId, userBalance);
//
//         // Act
//         Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         await act.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
//         _userRepositoryMock.Verify(x => x.UpdateUserBalanceAsync(userId, userBalance, It.IsAny<CancellationToken>()), Times.Once);
//     }
// }
