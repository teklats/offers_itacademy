// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Health.Queries;

namespace OffersPlatform.API.Controllers.v1.Health;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var status = await _mediator
            .Send(new HealthCheckQuery())
            .ConfigureAwait(false);
        return Ok(new
        {
            status,
            checkedAt = DateTime.Now
        });
    }
}
