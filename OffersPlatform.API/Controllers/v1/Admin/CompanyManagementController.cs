using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Admin.Companies.Commands.ApproveCompany;
using OffersPlatform.Application.Features.Admin.Companies.Commands.DeleteCompany;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllActiveCompanies;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllCompanies;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetCompanyById;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.API.Controllers.v1.Admin;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/admin/companies")]
public class CompanyManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCompanies(CancellationToken cancellationToken = default)
    {
        var query = new GetAllCompaniesQuery();
        var companies = await _mediator.Send(query, cancellationToken);
        return Ok(companies);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetAllActiveCompanies(CancellationToken cancellationToken)
    {
        var query = new GetAllActiveCompaniesQuery();
        var companies = await _mediator.Send(query, cancellationToken);
        return Ok(companies);
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompanyById(Guid companyId, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(companyId);
        var company = await _mediator.Send(query, cancellationToken);
        return Ok(company);
    }
    
    
    [HttpPut("{companyId}/approve")]
    public async Task<ActionResult> ApproveCompanyStatus(Guid companyId, CompanyStatus status,
        CancellationToken cancellationToken)
    {
        var command = new ApproveCompanyCommand(companyId, CompanyStatus.Active);
        
        var result = await _mediator.Send(command, cancellationToken);
    
        if (result)
        {
            return Ok("Company approved successfully.");
        }
        else
        {
            return BadRequest("Company is already active.");
        }
    }
    
    [HttpDelete("{companyId}")]
    public async Task<ActionResult> DeleteCompany(Guid companyId,
        CancellationToken cancellationToken)
    {
        var query = new DeleteCompanyCommand(companyId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
}