using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterCompany;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterUser;
using OffersPlatform.Application.Features.Auth.Commands.Login;


namespace OffersPlatform.API.Controllers.v1.Auth;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.
            Send(command, cancellationToken)
            .ConfigureAwait(false);
        return result != null ? Ok(result) : BadRequest(result);
    }

    [HttpPost("register/company")]
    public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return result != null ? Ok(result) : Unauthorized(result);
    }

}
