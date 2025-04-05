using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Admin.Users.Commands.DeleteUser;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetAllUser;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;

namespace OffersPlatform.API.Controllers.v1.Admin;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/admin/users")]
public class UsersManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUserQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return result != null ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return result != null ? Ok(result) : BadRequest(result);
    }
    
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);
        return Ok();
    }
}