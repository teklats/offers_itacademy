using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;

namespace OffersPlatform.API.Controllers.v1.Companies;

[ApiController]
[Authorize]
[Route("api/v1/companies/offers")]
public class CompanyOffersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyOffersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetCompanyId()
    {
        var currentCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentCompanyId, out var companyId))
        {
            throw new NotFoundException("Company Not Found");
        }
        return companyId;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanyAllOffers(CancellationToken cancellationToken)
    {

        var query = new GetAllOfferQuery(GetCompanyId());
        var result = await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOfferByIdQuery(GetCompanyId(), id);
        var result = await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffer([FromBody] OfferCreateDto request, CancellationToken cancellationToken)
    {

        var command = new CreateOfferCommand(request, GetCompanyId());
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> CancelOffer(Guid Id, CancellationToken cancellationToken)
    {
        var command = new CancelOfferCommand(GetCompanyId(), Id);
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    // [HttpPost("{id}/photo")]
    // public async Task<IActionResult> UploadOfferPhoto(Guid id)
    // {
    //     return Ok();
    // }
}
