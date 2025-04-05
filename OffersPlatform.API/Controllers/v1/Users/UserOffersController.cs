using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OffersPlatform.API.Controllers.v1.Users;

[ApiController]
[Route("api/v1/users")]
public class UserOffersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserOffersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("offers")]
    public async Task<IActionResult> GetActiveOffers()
    {
        return Ok();
    }

    [HttpGet("offers/{id}")]
    public async Task<IActionResult> GetOfferDetails(Guid id)
    {
        return Ok();
    }

    [HttpPost("offers/{id}/purchase")]
    public async Task<IActionResult> PurchaseOffer(Guid id)
    {
        return Ok();
    }

    [HttpDelete("offers/purchases/{id}")]
    public async Task<IActionResult> CancelPurchase(Guid id)
    {
        return Ok();
    }

    [HttpGet("purchases")]
    public async Task<IActionResult> GetPurchaseHistory()
    {
        return Ok();
    }

    [HttpGet("offers/archived")]
    public async Task<IActionResult> GetArchivedOffers()
    {
        return Ok();
    }
    
}