using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OffersPlatform.API.Controllers.v1.Admin;


[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/admin")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpGet("categories/{categoryId}")]
    public async Task<IActionResult> GetCategoryById(Guid categoryId, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory(Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpPost("categories/{categoryId}")]
    public async Task<IActionResult> UpdateCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpDelete("categories/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    
}