// Copyright (C) TBC Bank. All Rights Reserved.

using AutoMapper;
using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetUserPurchase;
using OffersPlatform.Domain.Entities;
using Xunit;

namespace OffersPlatform.Application.UnitTests.UserTest;
public class GetUserPurchaseHistoryQueryHandlerTests
    {
        private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetUserPurchaseHistoryQueryHandlerTests()
        {
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetUserPurchaseHistoryQueryHandler_ShouldReturnPurchases_WhenPurchasesExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var purchases = new List<Purchase>
            {
                new Purchase { UserId = userId, TotalPrice = 50m, PurchasedAt = DateTime.Now },
                new Purchase { UserId = userId, TotalPrice = 75m, PurchasedAt = DateTime.Now }
            };
            var purchaseDtos = new List<PurchaseDto>
            {
                new PurchaseDto { UserId = userId, TotalPrice = 50m, PurchasedAt = purchases[0].PurchasedAt },
                new PurchaseDto { UserId = userId, TotalPrice = 75m, PurchasedAt = purchases[1].PurchasedAt}
            };

            _purchaseRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchases);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<PurchaseDto>>(purchases))
                .Returns(purchaseDtos);

            var handler = new GetUserPurchaseHistoryQueryHandler(_purchaseRepositoryMock.Object, _mapperMock.Object);
            var query = new GetUserPurchaseHistoryQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(purchaseDtos);
            _purchaseRepositoryMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
