// // Copyright (C) TBC Bank. All Rights Reserved.
//
// using FluentAssertions;
// using Moq;
// using OffersPlatform.Application.Common.Interfaces.IRepositories;
// using OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;
// using OffersPlatform.Domain.Entities;
// using Xunit;
//
// namespace OffersPlatform.Application.UnitTests.CompanyTest;
//
// public class CancelOfferCommandHandlerTests
// {
//     private readonly Mock<IOfferRepository> _offerRepositoryMock;
//     private readonly Mock<IUserRepository> _userRepositoryMock;
//     private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
//     private readonly CancelOfferCommandHandler _handler;
//
//     public CancelOfferCommandHandlerTests()
//     {
//         _offerRepositoryMock = new Mock<IOfferRepository>();
//         _userRepositoryMock = new Mock<IUserRepository>();
//         _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
//         _handler = new CancelOfferCommandHandler(
//             _offerRepositoryMock.Object,
//             _userRepositoryMock.Object,
//             _purchaseRepositoryMock.Object
//         );
//     }
//
//     [Fact]
//     public async Task Handle_ShouldReturnFalse_WhenOfferIsNotFound()
//     {
//         // Arrange
//         var command = new CancelOfferCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         _offerRepositoryMock
//             .Setup(x => x.GetByIdAsync(command.OfferId, command.CompanyId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync((Offer)null); // Simulate that offer is not found
//
//         // Act
//         var result = await _handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().BeFalse();
//     }
//
//     [Fact]
//     public async Task Handle_ShouldReturnFalse_WhenOfferIsOlderThan10Minutes()
//     {
//         // Arrange
//         var command = new CancelOfferCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         var offer = new Offer
//         {
//             Id = command.OfferId,
//             CompanyId = command.CompanyId,
//             CreatedAt = DateTime.Now.AddMinutes(-15) // Older than 10 minutes
//         };
//
//         _offerRepositoryMock
//             .Setup(x => x.GetByIdAsync(command.OfferId, command.CompanyId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(offer);
//
//         // Act
//         var result = await _handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().BeFalse();
//     }
//
//     [Fact]
//     public async Task Handle_ShouldRefundUsersAndCancelOffer_WhenOfferIsValidAndCancelable()
//     {
//         // Arrange
//         var command = new CancelOfferCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         var offer = new Offer
//         {
//             Id = command.OfferId,
//             CompanyId = command.CompanyId,
//             CreatedAt = DateTime.Now.AddMinutes(-5) // Within 10 minutes
//         };
//
//         var purchase = new Purchase
//         {
//             Id = Guid.NewGuid(),
//             OfferId = command.OfferId,
//             TotalPrice = 100
//         };
//
//         var user = new User
//         {
//             Id = Guid.NewGuid(),
//             Balance = 50 // initial balance
//         };
//
//         _offerRepositoryMock
//             .Setup(x => x.GetByIdAsync(command.OfferId, command.CompanyId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(offer);
//
//         _purchaseRepositoryMock
//             .Setup(x => x.GetByOfferIdAsync(command.OfferId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new[] { purchase });
//
//         _purchaseRepositoryMock
//             .Setup(x => x.GetUserByPurchaseId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new[] { user.Id });
//
//         _userRepositoryMock
//             .Setup(x => x.GetActiveUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(user);
//
//         _userRepositoryMock
//             .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
//             .Returns(Task.CompletedTask);
//
//         _offerRepositoryMock
//             .Setup(x => x.CancelOfferAsync(command.OfferId, command.CompanyId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(true);
//
//         // Act
//         var result = await _handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().BeTrue();
//         user.Balance.Should().Be(150); // Refund added to user's balance
//     }
// }
