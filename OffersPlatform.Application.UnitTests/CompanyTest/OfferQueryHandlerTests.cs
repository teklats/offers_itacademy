// Copyright (C) TBC Bank. All Rights Reserved.

using AutoMapper;
using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;
using OffersPlatform.Domain.Entities;
using Xunit;

namespace OffersPlatform.Application.UnitTests.CompanyTest;

public class OfferQueryHandlerTests
{
    private readonly Mock<IOfferRepository> _offerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public OfferQueryHandlerTests()
    {
        _offerRepositoryMock = new Mock<IOfferRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task GetAllOfferQueryHandler_ShouldReturnOffers_WhenCompanyHasOffers()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var offers = new List<Offer>
        {
            new Offer { Id = Guid.NewGuid(), Name = "Offer1", CompanyId = companyId },
            new Offer { Id = Guid.NewGuid(), Name = "Offer2", CompanyId = companyId }
        };
        var offerDtos = new List<OfferDto>
        {
            new OfferDto { Id = offers[0].Id, Name = offers[0].Name },
            new OfferDto { Id = offers[1].Id, Name = offers[1].Name }
        };

        _offerRepositoryMock
            .Setup(x => x.GetAllOffersAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(offers);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<OfferDto>>(It.IsAny<IEnumerable<Offer>>()))
            .Returns(offerDtos);

        var handler = new GetAllOfferQueryHandler(_offerRepositoryMock.Object, _mapperMock.Object);
        var query = new GetAllOfferQuery(companyId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(offerDtos);
        _offerRepositoryMock.Verify(x => x.GetAllOffersAsync(companyId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOfferByIdQueryHandler_ShouldThrowNotFoundException_WhenOfferDoesNotExist()
    {
        // Arrange
        var offerId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        // Mock repository to return null for non-existent offer
        _offerRepositoryMock
            .Setup(x => x.GetByIdAsync(offerId, companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Offer)null);

        // Instantiate the handler with mocked dependencies
        var handler = new GetOfferByIdQueryHandler(_offerRepositoryMock.Object, _mapperMock.Object);
        var query = new GetOfferByIdQuery(offerId, companyId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }

}
