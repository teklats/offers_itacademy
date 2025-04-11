using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetAllUser;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetAllCompanies;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetAllCategories;
using OffersPlatform.Application.Features.Admin.Categories.Commands.CreateCategory;
using OffersPlatform.Application.Features.Admin.Companies.Commands.ApproveCompany;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Users(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUserQuery(), cancellationToken);
        return View(users);
    }

    public async Task<IActionResult> Companies(CancellationToken cancellationToken)
    {
        var companies = await _mediator.Send(new GetAllCompaniesQuery(), cancellationToken);
        return View(companies);
    }

    public async Task<IActionResult> Categories(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return View(categories);
    }

    // Action to activate a company
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ApproveCompany(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new ApproveCompanyCommand(id, CompanyStatus.Active), cancellationToken);

            TempData["Success"] = result ? "Company approved." : "Company is already active.";
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Companies));
    }


    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(string name, string description, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError(nameof(name), "Category name is required.");
            return View();
        }

        await _mediator.Send(new CreateCategoryCommand(name, description), cancellationToken);
        return RedirectToAction(nameof(Categories));
    }
}
