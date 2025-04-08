using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetCompanyById;

namespace OffersPlatform.API.Controllers.v1.Companies;

[ApiController]
[Route("api/v1/companies/profile")]
public class CompanyProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompanyProfile(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var company = await _mediator.Send(query, cancellationToken);
        return Ok(company);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCompanyProfile(Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPost("photo")]
    public async Task<IActionResult> UploadCompanyPhoto(Guid id, string imageUrl,CancellationToken cancellationToken)
    {
        return Ok();
    }
}