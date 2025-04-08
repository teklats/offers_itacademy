using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Admin.Categories.Commands.DeleteCategory;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetAllCategories;
using OffersPlatform.Application.Features.Users.Categories.Commands.AddCategories;
using OffersPlatform.Application.Features.Users.Categories.Commands.RemoveCategories;
using OffersPlatform.Application.Features.Users.Categories.Queries.GetUserCategories;

namespace OffersPlatform.API.Controllers.v1.Users;

[Authorize]
[ApiController]
[Route("/api/v1/users/preferred-categories")]
public class PreferredCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PreferredCategoriesController(IMediator mediator)
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
    public async Task<IActionResult> GetPreferredCategories(CancellationToken cancellationToken)
    {
        var command = new GetUserCategoriesQuery(GetUserId());
        var category = await _mediator.Send(command, cancellationToken);
        return Ok(category);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddCategoryToPreference(Guid categotyId, CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(GetUserId(), categotyId);
        var category = await _mediator.Send(command, cancellationToken);
        return Ok(category);

    }
    
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategoryFromPreference(Guid categoryId, CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(GetUserId(), categoryId);
        var category = await _mediator.Send(command, cancellationToken);
        return Ok(category);
    }
}