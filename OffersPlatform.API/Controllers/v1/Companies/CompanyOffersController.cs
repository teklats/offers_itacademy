using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;
using OffersPlatform.Domain.Entities;

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
    
    [HttpGet]
    public async Task<IActionResult> GetCompanyAllOffers()
    {
        var currentCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentCompanyId, out var companyId))
        {
            return Forbid("Invalid user ID.");
        }
        var query = new GetAllOfferQuery(companyId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferDetails(Guid id)
    {
        var currentCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentCompanyId, out var companyId))
        {
            return Forbid("Invalid user ID.");
        }
        var query = new GetOfferByIdQuery(companyId, id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffer([FromBody] OfferCreateDto request)
    {
        var currentCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentCompanyId, out var companyId))
        {
            return Forbid("Invalid user ID.");
        }
        var command = new CreateOfferCommand(request, companyId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> CancelOffer(Guid Id)
    {
        var currentCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentCompanyId, out var companyId))
        {
            return Forbid("Invalid user ID.");
        }
        var command = new CancelOfferCommand(companyId, Id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/photo")]
    public async Task<IActionResult> UploadOfferPhoto(Guid id)
    {
        return Ok();
    }
}