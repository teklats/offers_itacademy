// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Health.Queries;

public class HealthCheckHandler : IRequestHandler<HealthCheckQuery, HealthStatus>
{
    private readonly IDependencyHealthChecker _healthChecker;

    public HealthCheckHandler(IDependencyHealthChecker healthChecker)
    {
        _healthChecker = healthChecker;
    }

    public async Task<HealthStatus> Handle(HealthCheckQuery request, CancellationToken cancellationToken)
    {
        var isDbConnected = await _healthChecker
            .CanConnectToDatabaseAsync(cancellationToken)
            .ConfigureAwait(false);

        return new HealthStatus
        {
            Status = isDbConnected ? "Healthy" : "Unhealthy",
            DatabaseConnected = isDbConnected,
            CheckedAt = DateTime.Now,
            Message = isDbConnected ? "Connected to DB" : "Failed to connect to DB"
        };
    }
}
