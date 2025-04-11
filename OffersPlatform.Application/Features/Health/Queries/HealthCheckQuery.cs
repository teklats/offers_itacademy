// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Health.Queries;

public class HealthCheckQuery : IRequest<HealthStatus>
{
}
