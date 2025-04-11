// Copyright (C) TBC Bank. All Rights Reserved.

using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using Xunit;

namespace OffersPlatform.Application.UnitTests.UserTest;

public class CancelPurchaseCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IOfferRepository> _offerRepositoryMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CancelPurchaseCommandHandler _handler;

    public CancelPurchaseCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _offerRepositoryMock = new Mock<IOfferRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();

        _unitOfWorkMock.Setup(u => u.PurchaseRepository).Returns(_purchaseRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.OfferRepository).Returns(_offerRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(_companyRepositoryMock.Object);

        _handler = new CancelPurchaseCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCancelPurchase_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var purchase = new Purchase
        {
            Id = purchaseId,
            UserId = userId,
            Quantity = 5,
            TotalPrice = 50m,
            Status = PurchaseStatus.Completed,
            PurchasedAt = DateTime.Now
        };
        var user = new User
        {
            Id = userId,
            Balance = 100m
        };
        var offer = new Offer
        {
            Id = purchase.OfferId,
            AvailableQuantity = 10,
            UnitPrice = 10m,
            Status = OfferStatus.Active,
            CompanyId = Guid.NewGuid()
        };
        var company = new Company
        {
            Id = offer.CompanyId,
            Balance = 1000m
        };

        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository calls
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);
        _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _offerRepositoryMock.Setup(r => r.GetByIdAsync(purchase.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _companyRepositoryMock.Setup(r => r.GetCompanyByIdAsync(offer.CompanyId, It.IsAny<CancellationToken>())).ReturnsAsync(company);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _purchaseRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Purchase>(), It.IsAny<CancellationToken>()), Times.Once);
        _offerRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Offer>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _companyRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPurchaseNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository to return null for purchase
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync((Purchase)null);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Purchase Not Found");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCancelTimeWindowExceeded()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var purchase = new Purchase
        {
            Id = purchaseId,
            UserId = userId,
            Quantity = 5,
            TotalPrice = 50m,
            Status = PurchaseStatus.Completed,
            PurchasedAt = DateTime.Now.AddMinutes(-6) // Exceeds the 5-minute window
        };
        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository calls
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().WithMessage("Purchase can only be canceled within 5 minutes.");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var purchase = new Purchase
        {
            Id = purchaseId,
            UserId = userId,
            Quantity = 5,
            TotalPrice = 50m,
            Status = PurchaseStatus.Completed,
            PurchasedAt = DateTime.Now
        };
        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository calls
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);
        _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("User Not Found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenOfferNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var purchase = new Purchase
        {
            Id = purchaseId,
            UserId = userId,
            Quantity = 5,
            TotalPrice = 50m,
            Status = PurchaseStatus.Completed,
            PurchasedAt = DateTime.Now
        };
        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository calls
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);
        _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new User { Id = userId });
        _offerRepositoryMock.Setup(r => r.GetByIdAsync(purchase.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync((Offer)null);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Offer Not Found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCompanyNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var purchase = new Purchase
        {
            Id = purchaseId,
            UserId = userId,
            Quantity = 5,
            TotalPrice = 50m,
            Status = PurchaseStatus.Completed,
            PurchasedAt = DateTime.Now
        };
        var command = new CancelPurchaseCommand(userId, purchaseId);

        // Mock repository calls
        _purchaseRepositoryMock.Setup(r => r.GetByIdAsync(purchaseId, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);
        _userRepositoryMock.Setup(r => r.GetActiveUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new User { Id = userId });
        _offerRepositoryMock.Setup(r => r.GetByIdAsync(purchase.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(new Offer { CompanyId = Guid.NewGuid() });
        _companyRepositoryMock.Setup(r => r.GetCompanyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Company)null);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Company Not Found");
    }
}
