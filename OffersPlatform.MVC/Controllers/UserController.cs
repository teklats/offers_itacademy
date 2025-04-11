using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Features.Admin.Users.Commands.DeleteUser;
using OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;
using OffersPlatform.Application.Features.Users.Categories.Commands.AddCategories;
using OffersPlatform.Application.Features.Users.Categories.Commands.RemoveCategories;
using OffersPlatform.Application.Features.Users.Categories.Queries.GetUserCategories;
using OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;
using OffersPlatform.Application.Features.Users.Offers.Commands.PurchaseOffer;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetPreferredActiveOffers;
using OffersPlatform.Application.Features.Users.Offers.Queries.GetUserPurchase;
using OffersPlatform.Application.Features.Users.Profile.Commands.UpdateUserBalance;
using OffersPlatform.Application.Features.Admin.Categories.Queries.GetAllCategories;
using OffersPlatform.MVC.Models;


namespace OffersPlatform.MVC.Controllers;

[Authorize(Roles = "Customer")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var user = await _mediator
            .Send(new GetUserByIdQuery(userId), cancellationToken)
            .ConfigureAwait(false);

        var selected = await _mediator
            .Send(new GetUserCategoriesQuery(userId), cancellationToken)
            .ConfigureAwait(false);

        var offers = await _mediator
            .Send(new GetPreferredActiveOffersQuery(userId), cancellationToken)
            .ConfigureAwait(false);

        var all = await _mediator
            .Send(new GetAllCategoriesQuery(), cancellationToken)
            .ConfigureAwait(false);

        var purchases = await _mediator
            .Send(new GetUserPurchaseHistoryQuery(userId), cancellationToken)
            .ConfigureAwait(false);

        var model = new UserDashboardViewModel
        {
            User = user,
            SelectedCategories = selected.ToList(),
            AllCategories = all.ToList(),
            Offers = offers.ToList(),
            Purchases = purchases.ToList() // ✅ fixed line
        };

        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> AddBalance(UserDashboardViewModel model, CancellationToken cancellationToken)
    {
        if (model.BalanceToAdd is > 0)
        {
            await _mediator
                .Send(new UpdateUserBalanceCommand(GetUserId(), model.BalanceToAdd.Value), cancellationToken)
                .ConfigureAwait(false);
            TempData["SuccessMessage"] = "Balance updated.";
        }

        return RedirectToAction(nameof(Profile));
    }


    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(CancellationToken cancellationToken)
    {
        await _mediator
            .Send(new DeleteUserCommand(GetUserId()), cancellationToken)
            .ConfigureAwait(false);
        return RedirectToAction("Logout", "Auth"); // Or wherever logout lands
    }

    public async Task<IActionResult> PreferredCategories(CancellationToken cancellationToken)
    {
        var selected = await _mediator
            .Send(new GetUserCategoriesQuery(GetUserId()), cancellationToken)
            .ConfigureAwait(false);
        var all = await _mediator
            .Send(new GetAllCategoriesQuery(), cancellationToken)
            .ConfigureAwait(false);

        var model = new CategoryPreferencesViewModel
        {
            SelectedCategoryIds = selected.Select(x => x.Id).ToList(),
            AllCategories = all.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddPreferredCategory(Guid categoryIdToAdd, CancellationToken cancellationToken)
    {
        await _mediator
            .Send(new AddCategoryCommand(GetUserId(), categoryIdToAdd), cancellationToken)
            .ConfigureAwait(false);
        TempData["SuccessMessage"] = "Category added!";
        return RedirectToAction(nameof(Profile));
    }



    [HttpPost]
    public async Task<IActionResult> RemovePreferredCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        await _mediator
            .Send(new RemoveCategoryCommand(GetUserId(), categoryId), cancellationToken)
            .ConfigureAwait(false);
        TempData["SuccessMessage"] = "Category removed.";
        return RedirectToAction(nameof(Profile));
    }

    public async Task<IActionResult> Offers(CancellationToken cancellationToken)
    {
        var offers = await _mediator
            .Send(new GetPreferredActiveOffersQuery(GetUserId()), cancellationToken)
            .ConfigureAwait(false);
        return View(offers);
    }

    [HttpPost]
    public async Task<IActionResult> PurchaseOffer(Guid offerId, int quantity, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator
                .Send(new PurchaseOfferCommand(GetUserId(), offerId, quantity), cancellationToken)
                .ConfigureAwait(false);
            TempData["SuccessMessage"] = "Offer purchased successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("Profile");
    }


    public async Task<IActionResult> PurchaseHistory(CancellationToken cancellationToken)
    {
        var purchases = await _mediator.Send(new GetUserPurchaseHistoryQuery(GetUserId()), cancellationToken);
        return View(purchases);
    }

    [HttpPost]
    public async Task<IActionResult> CancelPurchase(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelPurchaseCommand(GetUserId(), id), cancellationToken);
        TempData["SuccessMessage"] = "Purchase cancelled.";
        return RedirectToAction(nameof(PurchaseHistory));
    }
}
