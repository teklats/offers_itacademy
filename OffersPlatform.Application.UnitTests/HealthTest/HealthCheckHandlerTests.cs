// Copyright (C) TBC Bank. All Rights Reserved.

using FluentAssertions;
using Moq;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Features.Health.Queries;
using Xunit;

namespace OffersPlatform.Application.UnitTests.HealthTest;

public class HealthCheckHandlerTests
{
    [Fact]
    public async Task HealthCheckHandler_ShouldReturnHealthy_WhenDatabaseIsConnected()
    {
        // Arrange
        var healthCheckerMock = new Mock<IDependencyHealthChecker>();
        healthCheckerMock.Setup(h => h.CanConnectToDatabaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new HealthCheckHandler(healthCheckerMock.Object);
        var query = new HealthCheckQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be("Healthy");
        result.DatabaseConnected.Should().BeTrue();
        result.Message.Should().Be("Connected to DB");
    }

    [Fact]
    public async Task HealthCheckHandler_ShouldReturnUnhealthy_WhenDatabaseIsNotConnected()
    {
        // Arrange
        var healthCheckerMock = new Mock<IDependencyHealthChecker>();
        healthCheckerMock.Setup(h => h.CanConnectToDatabaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new HealthCheckHandler(healthCheckerMock.Object);
        var query = new HealthCheckQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be("Unhealthy");
        result.DatabaseConnected.Should().BeFalse();
        result.Message.Should().Be("Failed to connect to DB");
    }
}
