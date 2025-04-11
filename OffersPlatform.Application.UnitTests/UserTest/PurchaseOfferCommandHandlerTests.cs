// // Copyright (C) TBC Bank. All Rights Reserved.
//
// using AutoMapper;
// using FluentAssertions;
// using Moq;
// using OffersPlatform.Application.Common.Interfaces;
// using OffersPlatform.Application.Common.Interfaces.IRepositories;
// using OffersPlatform.Application.DTOs;
// using OffersPlatform.Application.Exceptions;
// using OffersPlatform.Application.Features.Users.Offers.Commands.PurchaseOffer;
// using OffersPlatform.Domain.Entities;
// using OffersPlatform.Domain.Enums;
// using Xunit;
//
// namespace OffersPlatform.Application.UnitTests.UserTest;
// public class PurchaseOfferCommandHandlerTests
// {
//     private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//     private readonly Mock<IOfferRepository> _offerRepositoryMock;
//     private readonly Mock<IUserRepository> _userRepositoryMock;
//     private readonly Mock<ICompanyRepository> _companyRepositoryMock;
//     private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
//     private readonly IMapper _mapper;
//
//     public PurchaseOfferCommandHandlerTests()
//     {
//         _unitOfWorkMock = new Mock<IUnitOfWork>();
//         _offerRepositoryMock = new Mock<IOfferRepository>();
//         _userRepositoryMock = new Mock<IUserRepository>();
//         _companyRepositoryMock = new Mock<ICompanyRepository>();
//         _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
//
//         _unitOfWorkMock.Setup(u => u.OfferRepository).Returns(_offerRepositoryMock.Object);
//         _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);
//         _unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(_companyRepositoryMock.Object);
//         _unitOfWorkMock.Setup(u => u.PurchaseRepository).Returns(_purchaseRepositoryMock.Object);
//
//         var config = new MapperConfiguration(cfg =>
//         {
//             cfg.CreateMap<Purchase, PurchaseDto>();
//         });
//
//         _mapper = config.CreateMapper();
//     }
//
//     [Fact]
//     public async Task Handle_ShouldPurchaseOffer_WhenOfferAndUserAreValid()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var offerId = Guid.NewGuid();
//         var quantity = 5;
//         var totalPrice = 50m;
//         var offer = new Offer
//         {
//             Id = offerId,
//             Name = "Test Offer",
//             UnitPrice = 10m,
//             AvailableQuantity = 10,
//             Status = OfferStatus.Active,
//             ExpiresAt = DateTime.Now.AddDays(1),
//             CompanyId = Guid.NewGuid()
//         };
//         var user = new User
//         {
//             Id = userId,
//             Balance = 100m
//         };
//         var company = new Company
//         {
//             Id = offer.CompanyId,
//             Balance = 1000m
//         };
//
//         var command = new PurchaseOfferCommand(userId, offerId, quantity);
//
//         _offerRepositoryMock.Setup(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
//         _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
//         _companyRepositoryMock.Setup(r => r.GetCompanyByIdAsync(offer.CompanyId, It.IsAny<CancellationToken>())).ReturnsAsync(company);
//
//         var handler = new PurchaseOfferCommandHandler(_unitOfWorkMock.Object, _mapper);
//
//         // Act
//         var result = await handler.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.UserId.Should().Be(userId);
//         result.OfferId.Should().Be(offerId);
//         result.Quantity.Should().Be(quantity);
//         result.TotalPrice.Should().Be(totalPrice);
//
//         // Verify repository interactions
//         _offerRepositoryMock.Verify(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>()), Times.Once);
//         _userRepositoryMock.Verify(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
//         _companyRepositoryMock.Verify(r => r.GetCompanyByIdAsync(offer.CompanyId, It.IsAny<CancellationToken>()), Times.Once);
//         _purchaseRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Purchase>(), It.IsAny<CancellationToken>()), Times.Once);
//     }
//
//     [Fact]
//     public async Task Handle_ShouldThrowException_WhenUserDoesNotHaveEnoughBalance()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var offerId = Guid.NewGuid();
//         var quantity = 5;
//         var offer = new Offer
//         {
//             Id = offerId,
//             Name = "Test Offer",
//             UnitPrice = 10m,
//             AvailableQuantity = 10,
//             Status = OfferStatus.Active,
//             ExpiresAt = DateTime.Now.AddDays(1),
//             CompanyId = Guid.NewGuid()
//         };
//         var user = new User
//         {
//             Id = userId,
//             Balance = 30m // Insufficient balance
//         };
//
//         var command = new PurchaseOfferCommand(userId, offerId, quantity);
//
//         _offerRepositoryMock.Setup(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
//         _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
//
//         var handler = new PurchaseOfferCommandHandler(_unitOfWorkMock.Object, _mapper);
//
//         // Act & Assert
//         Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
//         await act.Should().ThrowAsync<Exception>().WithMessage("User does not have enough balance");
//     }
//
//     [Fact]
//     public async Task Handle_ShouldThrowException_WhenOfferIsExpired()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var offerId = Guid.NewGuid();
//         var quantity = 5;
//         var offer = new Offer
//         {
//             Id = offerId,
//             Name = "Test Offer",
//             UnitPrice = 10m,
//             AvailableQuantity = 10,
//             Status = OfferStatus.Active,
//             ExpiresAt = DateTime.Now.AddDays(-1), // Expired offer
//             CompanyId = Guid.NewGuid()
//         };
//         var user = new User
//         {
//             Id = userId,
//             Balance = 100m
//         };
//
//         var command = new PurchaseOfferCommand(userId, offerId, quantity);
//
//         _offerRepositoryMock.Setup(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
//         _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
//
//         var handler = new PurchaseOfferCommandHandler(_unitOfWorkMock.Object, _mapper);
//
//         // Act & Assert
//         Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
//         await act.Should().ThrowAsync<Exception>().WithMessage("Offer is not available");
//     }
//
//     [Fact]
//     public async Task Handle_ShouldThrowException_WhenNotEnoughQuantityAvailable()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var offerId = Guid.NewGuid();
//         var quantity = 15; // More than available
//         var offer = new Offer
//         {
//             Id = offerId,
//             Name = "Test Offer",
//             UnitPrice = 10m,
//             AvailableQuantity = 10,
//             Status = OfferStatus.Active,
//             ExpiresAt = DateTime.Now.AddDays(1),
//             CompanyId = Guid.NewGuid()
//         };
//         var user = new User
//         {
//             Id = userId,
//             Balance = 100m
//         };
//
//         var command = new PurchaseOfferCommand(userId, offerId, quantity);
//
//         _offerRepositoryMock.Setup(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
//         _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
//
//         var handler = new PurchaseOfferCommandHandler(_unitOfWorkMock.Object, _mapper);
//
//         // Act & Assert
//         Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
//         await act.Should().ThrowAsync<Exception>().WithMessage("Not enough quantity available");
//     }
// }
