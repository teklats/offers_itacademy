// // Copyright (C) TBC Bank. All Rights Reserved.
//
// using AutoMapper;
// using FluentAssertions;
// using Moq;
// using OffersPlatform.Application.Common.Interfaces.IRepositories;
// using OffersPlatform.Application.DTOs;
// using OffersPlatform.Application.Exceptions;
// using OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;
// using OffersPlatform.Domain.Entities;
// using OffersPlatform.Domain.Enums;
// using Xunit;
//
// namespace OffersPlatform.Application.UnitTests.CompanyTest;
//
// public class CreateOfferCommandHandlerTests
//     {
//         private readonly Mock<IOfferRepository> _offerRepositoryMock;
//         private readonly Mock<ICompanyRepository> _companyRepositoryMock;
//         private readonly Mock<IMapper> _mapperMock;
//         private readonly CreateOfferCommandHandler _handler;
//
//         public CreateOfferCommandHandlerTests()
//         {
//             _offerRepositoryMock = new Mock<IOfferRepository>();
//             _companyRepositoryMock = new Mock<ICompanyRepository>();
//             _mapperMock = new Mock<IMapper>();
//             _handler = new CreateOfferCommandHandler(
//                 _offerRepositoryMock.Object,
//                 _companyRepositoryMock.Object,
//                 _mapperMock.Object
//             );
//         }
//
//         [Fact]
//         public async Task Handle_ShouldThrowForbiddenException_WhenCompanyIsNotActive()
//         {
//             // Arrange
//             var command = new CreateOfferCommand(
//                 new OfferCreateDto
//                 {
//                     Name = "Offer 1",
//                     Description = "Offer Description",
//                     UnitPrice = 10,
//                     InitialQuantity = 100,
//                     AvailableQuantity = 100,
//                     ExpiresAt = DateTime.Now.AddDays(1),
//                     CategoryId = Guid.NewGuid()
//                 },
//                 Guid.NewGuid() // Simulating a company ID
//             );
//
//             _companyRepositoryMock
//                 .Setup(x => x.CompanyIsActiveAsync(command.CompanyId, It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(false); // Simulating inactive company
//
//             // Act & Assert
//             await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));
//         }
//
//         [Fact]
//         public async Task Handle_ShouldCreateOffer_WhenCompanyIsActive()
//         {
//             // Arrange
//             var command = new CreateOfferCommand(
//                 new OfferCreateDto
//                 {
//                     Name = "Offer 1",
//                     Description = "Offer Description",
//                     UnitPrice = 10,
//                     InitialQuantity = 100,
//                     AvailableQuantity = 100,
//                     ExpiresAt = DateTime.Now.AddDays(1),
//                     CategoryId = Guid.NewGuid()
//                 },
//                 Guid.NewGuid() // Simulating a company ID
//             );
//
//             _companyRepositoryMock
//                 .Setup(x => x.CompanyIsActiveAsync(command.CompanyId, It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(true); // Simulating active company
//
//             var offerResultDto = new OfferResultDto
//             {
//                 Id = Guid.NewGuid(),
//                 Name = command.Name,
//                 Description = command.Description,
//                 UnitPrice = command.UnitPrice,
//                 InitialQuantity = command.InitialQuantity,
//                 AvailableQuantity = command.AvailableQuantity,
//                 ExpiresAt = command.ExpiresAt,
//                 CategoryId = command.CategoryId,
//                 CompanyId = command.CompanyId,
//                 Status = OfferStatus.Active
//             };
//
//             _mapperMock
//                 .Setup(m => m.Map<Offer>(It.IsAny<OfferResultDto>()))
//                 .Returns(new Offer
//                 {
//                     Id = offerResultDto.Id,
//                     Name = offerResultDto.Name,
//                     Description = offerResultDto.Description,
//                     UnitPrice = offerResultDto.UnitPrice,
//                     InitialQuantity = offerResultDto.InitialQuantity,
//                     AvailableQuantity = offerResultDto.AvailableQuantity,
//                     ExpiresAt = offerResultDto.ExpiresAt,
//                     CategoryId = offerResultDto.CategoryId,
//                     CompanyId = offerResultDto.CompanyId,
//                     Status = offerResultDto.Status
//                 });
//
//             _offerRepositoryMock
//                 .Setup(x => x.AddAsync(It.IsAny<Offer>(), It.IsAny<CancellationToken>()))
//                 .Returns(Task.CompletedTask); // Simulate adding offer to repository
//
//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);
//
//             // Assert
//             result.Should().NotBeNull();
//             result.Name.Should().Be(command.Name);
//             result.Description.Should().Be(command.Description);
//             result.UnitPrice.Should().Be(command.UnitPrice);
//         }
//     }
