using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Admin.Categories.Commands.CreateCategory;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetAllCategories;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetCategoryById;

namespace OffersPlatform.API.Controllers.v1.Admin;


[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/admin/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById(Guid categoryId, CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(categoryId);
        var result = await _mediator
            .Send(query, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddCategory(string name, string description, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(name, description);
        var result = await _mediator
            .Send(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }

    //
    // [HttpDelete("{categoryId}")]
    // public async Task<IActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
    // {
    //     return Ok();
    // }

}
