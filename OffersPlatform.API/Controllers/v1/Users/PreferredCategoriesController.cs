using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OffersPlatform.API.Controllers.v1.Users;

[ApiController]
[Route("/api/v1/users/preferred-categories")]
public class PreferredCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PreferredCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPreferredCategories(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPost("")]
    public async Task<IActionResult> AddCategoryToPreference(CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpDelete("/{categoryId}")]
    public async Task<IActionResult> DeleteCategoryFromPreference(Guid categoryId, CancellationToken cancellationToken)
    {
        return Ok();
    }
}