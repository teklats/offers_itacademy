using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Admin.Users.Commands.DeleteUser;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;

namespace OffersPlatform.API.Controllers.v1.Users;

[ApiController]
[Route("api/v1/users/profile")]
public class UserProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpGet("")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query, cancellationToken);
        return Ok(user);
    }
    
    [Authorize (Roles = "Admin, Customer")]
    [HttpPut("")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UserUpdateDto userUpdateDto)
    {
        return Ok();
    }
    
    [Authorize (Roles = "Admin, Customer")]
    [HttpDelete("")]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken)
    {
        var query = new DeleteUserCommand(id);
        await _mediator.Send(query, cancellationToken);
        return Ok();
    }
}
