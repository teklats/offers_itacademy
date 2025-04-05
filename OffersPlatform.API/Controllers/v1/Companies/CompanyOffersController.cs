using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OffersPlatform.API.Controllers.v1.Companies;

[ApiController]
[Route("api/v1/companies")]
public class CompanyOffersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyOffersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("offers")]
    public async Task<IActionResult> GetCompanyOffers()
    {
        return Ok();
    }

    [HttpGet("offers/{id}")]
    public async Task<IActionResult> GetOfferDetails(Guid id)
    {
        return Ok();
    }

    [HttpPost("offers")]
    public async Task<IActionResult> CreateOffer()
    {
        return Ok();
    }

    [HttpPut("offers/{id}")]
    public async Task<IActionResult> UpdateOffer(Guid id)
    {
        return Ok();
    }

    [HttpDelete("offers/{id}")]
    public async Task<IActionResult> CancelOffer(Guid id)
    {
        return Ok();
    }

    [HttpGet("offers/archived")]
    public async Task<IActionResult> GetArchivedOffers()
    {
        return Ok();
    }

    [HttpPost("offers/{id}/photo")]
    public async Task<IActionResult> UploadOfferPhoto(Guid id)
    {
        return Ok();
    }
}