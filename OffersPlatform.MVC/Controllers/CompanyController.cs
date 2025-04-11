// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Admin.Companies.Queries.GetCompanyById;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetAllCategories;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;
using OffersPlatform.Application.Features.Companies.Profile.Commands;
using OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOfferPurchases;
using OffersPlatform.MVC.Models;

namespace OffersPlatform.MVC.Controllers;

[Authorize(Roles = "Company")]
public class CompanyController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IMediator _mediator;

    public CompanyController(IWebHostEnvironment env, IMediator mediator)
    {
        _env = env;
        _mediator = mediator;
    }

    private Guid GetCompanyId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(id, out var guid))
            throw new NotFoundException("Company not found");
        return guid;
    }

    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var companyId = GetCompanyId();
        var company = await _mediator.Send(new GetCompanyByIdQuery(companyId), cancellationToken).ConfigureAwait(false);
        var offers = await _mediator.Send(new GetAllOfferQuery(companyId), cancellationToken).ConfigureAwait(false);
        var purchases = await _mediator.Send(new GetCompanyOfferPurchasesQuery(companyId), cancellationToken).ConfigureAwait(false);

        var model = new CompanyDashboardViewModel
        {
            Company = company,
            Offers = offers.ToList(),
            Purchases = purchases.ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOffer(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken).ConfigureAwait(false);
        return View(new OfferCreateViewModel
        {
            Categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(OfferCreateViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        string? imageUrl = null;

        if (model.ImageFile is { Length: > 0 })
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/offers");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.ImageFile.CopyToAsync(stream, cancellationToken)
                .ConfigureAwait(false);

            imageUrl = $"/uploads/offers/{fileName}";
        }

        var createOfferDto = new OfferCreateDto
        {
            Name = model.Name,
            Description = model.Description,
            CategoryId = model.CategoryId,
            UnitPrice = model.Price,
            InitialQuantity = model.InitialQuantity,
            AvailableQuantity = model.InitialQuantity,
            ExpiresAt = model.ExpiresAt,
            ImageUrl = imageUrl,
        };

        var command = new CreateOfferCommand(createOfferDto, GetCompanyId());

        try
        {
            await _mediator.Send(command, cancellationToken)
                .ConfigureAwait(false);
            TempData["SuccessMessage"] = "Offer created successfully!";
            return RedirectToAction("Profile");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Profile");
        }
    }

    public async Task<IActionResult> OfferDetails(Guid id, CancellationToken cancellationToken)
    {
        var offer = await _mediator.Send(new GetOfferByIdQuery(GetCompanyId(), id), cancellationToken)
            .ConfigureAwait(false);
        return View(offer);
    }

    [HttpPost]
    public async Task<IActionResult> CancelOffer(Guid offerId, CancellationToken cancellationToken)
    {
        if (offerId == Guid.Empty)
        {
            TempData["ErrorMessage"] = "Invalid offer ID.";
            return RedirectToAction(nameof(Profile));
        }

        await _mediator.Send(new CancelOfferCommand(GetCompanyId(), offerId), cancellationToken)
            .ConfigureAwait(false);
        TempData["SuccessMessage"] = "Offer and associated purchases refunded.";
        return RedirectToAction(nameof(Profile));
    }


    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile imageFile, CancellationToken cancellationToken)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            TempData["ErrorMessage"] = "No file selected.";
            return RedirectToAction("Profile");
        }

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/companies");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream, cancellationToken)
            .ConfigureAwait(false);

        var imageUrl = $"/uploads/companies/{fileName}";

        await _mediator.Send(new UploadCompanyImageCommand(GetCompanyId(), imageUrl), cancellationToken)
            .ConfigureAwait(false);
        TempData["SuccessMessage"] = "Company image uploaded.";
        return RedirectToAction("Profile");
    }
}
