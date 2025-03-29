using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Users.Commands.CreateUser;

namespace OffersPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        // return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { Id = userId });
        return Ok(userId);
    }
}