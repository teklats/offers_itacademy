using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Admin.Users.Commands.DeleteUser;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;
using OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;

namespace OffersPlatform.API.Controllers.v1.Users;

[ApiController]
[Authorize]
[Route("api/v1/users/profile")]
public class UserProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserProfileController(IMediator mediator)
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

    [HttpGet("")]
    public async Task<IActionResult> GetUserProfile(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok(user);
    }

    [HttpPut("balance")]
    public async Task<IActionResult> AddToBalance(decimal balance, CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserBalanceCommand(GetUserId(), balance);
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpDelete("")]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken)
    {
        if (id == GetUserId())
        {
            return BadRequest("Invalid user id");
        }
        var query = new DeleteUserCommand(id);
        await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok();
    }

}
