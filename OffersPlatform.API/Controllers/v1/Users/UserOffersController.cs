using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;
using OffersPlatform.Application.Features.Users.Offers.Commands.PurchaseOffer;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetPreferredActiveOffers;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetUserPurchase;

namespace OffersPlatform.API.Controllers.v1.Users;

[ApiController]
[Authorize(Roles = "Customer")]
[Route("api/v1/users")]
public class UserOffersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserOffersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            throw new Exception("Invalid user id");
        }
        return Guid.Parse(userId);
    }
    
    [HttpGet("offers")]
    public async Task<IActionResult> GetPreferredActiveOffers()
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new GetPreferredActiveOffersQuery(userId));
        return Ok(result);
    }
    
    [HttpPost("offers/{id}/purchase")]
    public async Task<IActionResult> PurchaseOffer(Guid id, [FromQuery] int quantity)
    {
        var userId = GetUserId();
        var purchase = await _mediator.Send(new PurchaseOfferCommand(userId, id, quantity));
        return Ok(purchase);
    }
    
    [HttpDelete("offers/purchases/{id}")]
    public async Task<IActionResult> CancelPurchase(Guid id)
    {
        var userId = GetUserId();
        var cancel = await _mediator.Send(new CancelPurchaseCommand(userId, id));
        return Ok(cancel);
    }

 
    [HttpGet("purchases")]
    public async Task<IActionResult> GetPurchaseHistory()
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new GetUserPurchaseHistoryQuery(userId));
        return Ok(result);
    }
    
}