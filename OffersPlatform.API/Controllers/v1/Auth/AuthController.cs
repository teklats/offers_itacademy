using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.LoginCompany;
using OffersPlatform.Application.Features.Auth.CompanyAuth.Commands.RegisterCompany;
using OffersPlatform.Application.Features.Auth.UserAuth.Commands.LoginUser;
using OffersPlatform.Application.Features.Auth.UserAuth.Commands.RegisterUser;


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

    [HttpPost("login/user")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return result != null ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("login/company")]
    public async Task<IActionResult> LoginCompany([FromBody] LoginCompanyCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

}
