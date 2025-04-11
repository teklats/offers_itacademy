// Copyright (C) TBC Bank. All Rights Reserved.

using AutoMapper;
using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetPreferredActiveOffers;
using OffersPlatform.Domain.Entities;
using Xunit;

namespace OffersPlatform.Application.UnitTests.UserTest;

public class GetPreferredActiveOffersQueryHandlerTests
    {
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly Mock<IUserCategoryRepository> _userCategoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetPreferredActiveOffersQueryHandlerTests()
        {
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _userCategoryRepositoryMock = new Mock<IUserCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnOffers_WhenUserHasCategories()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId1 = Guid.NewGuid();
            var categoryId2 = Guid.NewGuid();
            var userCategories = new List<UserCategory>
            {
                new UserCategory { UserId = userId, CategoryId = categoryId1 },
                new UserCategory { UserId = userId, CategoryId = categoryId2 }
            };
            var offers = new List<Offer>
            {
                new Offer { Id = Guid.NewGuid(), CategoryId = categoryId1, Name = "Offer 1" },
                new Offer { Id = Guid.NewGuid(), CategoryId = categoryId2, Name = "Offer 2" }
            };
            var offerDtos = new List<OfferDto>
            {
                new OfferDto { Id = offers[0].Id, Name = "Offer 1" },
                new OfferDto { Id = offers[1].Id, Name = "Offer 2" }
            };

            _userCategoryRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userCategories);

            _offerRepositoryMock
                .Setup(x => x.GetByCategoryIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(offers);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<OfferDto>>(It.IsAny<IEnumerable<Offer>>()))
                .Returns(offerDtos);

            var handler = new GetPreferredActiveOffersQueryHandler(
                _offerRepositoryMock.Object,
                _mapperMock.Object,
                _userCategoryRepositoryMock.Object);
            var query = new GetPreferredActiveOffersQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(offerDtos);
            _offerRepositoryMock.Verify(x => x.GetByCategoryIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<OfferDto>>(It.IsAny<IEnumerable<Offer>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenUserHasNoCategories()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userCategories = new List<UserCategory>();

            _userCategoryRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userCategories);

            var handler = new GetPreferredActiveOffersQueryHandler(
                _offerRepositoryMock.Object,
                _mapperMock.Object,
                _userCategoryRepositoryMock.Object);
            var query = new GetPreferredActiveOffersQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
            _offerRepositoryMock.Verify(x => x.GetByCategoryIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoOffersForUserCategories()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var userCategories = new List<UserCategory>
            {
                new UserCategory { UserId = userId, CategoryId = categoryId }
            };
            var offers = new List<Offer>(); // No offers for the category

            _userCategoryRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userCategories);

            _offerRepositoryMock
                .Setup(x => x.GetByCategoryIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(offers);

            var handler = new GetPreferredActiveOffersQueryHandler(
                _offerRepositoryMock.Object,
                _mapperMock.Object,
                _userCategoryRepositoryMock.Object);
            var query = new GetPreferredActiveOffersQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
            _offerRepositoryMock.Verify(x => x.GetByCategoryIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
