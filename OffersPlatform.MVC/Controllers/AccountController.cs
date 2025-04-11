using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OffersPlatform.Application.Features.Auth.Commands.Login;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterUser;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterCompany;
using OffersPlatform.MVC.Models;
using System.Security.Claims;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var authResult = await _mediator.Send(new LoginCommand
            {
                Email = model.Email,
                Password = model.Password
            }).ConfigureAwait(false);

            if (authResult == null)
            {
                // This will display a custom error message on the same page.
                TempData["ErrorMessage"] = "Invalid password. Please try again.";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authResult.Id.ToString()),
                new Claim(ClaimTypes.Email, authResult.Email),
                new Claim(ClaimTypes.Role, authResult.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps)
                .ConfigureAwait(false);

            // Redirect based on the role
            if (authResult.Role == UserRole.Company)
            {
                return RedirectToAction("Profile", "Company");
            }
            else if (authResult.Role == UserRole.Customer)
            {
                return RedirectToAction("Profile", "User");
            }
            else if (authResult.Role == UserRole.Admin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;  // Pass the error message to TempData for display
            return View(model);  // Stay on the same page
        }
    }


    [HttpGet]
    public IActionResult RegisterUser()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var command = new RegisterUserCommand
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.UserName,
            Email = model.Email,
            Password = model.Password,
            PhoneNumber = model.PhoneNumber,
        };

        try
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);

            if (result == null)
            {
                // Store the error message in TempData for display
                TempData["ErrorMessage"] = "User registration failed. Please try again.";
                return View();
            }

            // Redirect to login page after successful registration
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            // Catch any exceptions and pass the error message to TempData
            TempData["ErrorMessage"] = ex.Message;
            return View();  // Stay on the same page and display the error
        }
    }

    [HttpGet]
    public IActionResult RegisterCompany()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var command = new RegisterCompanyCommand
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password,
            Description = model.Description,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address
        };

        try
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);

            if (result == null)
            {
                // Store the error message in TempData for display
                TempData["ErrorMessage"] = "Company registration failed. Please try again.";
                return View();
            }

            // Redirect to login page after successful registration
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            // Catch any exceptions and pass the error message to TempData
            TempData["ErrorMessage"] = ex.Message;
            return View();  // Stay on the same page and display the error
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            .ConfigureAwait(false);
        return RedirectToAction("Login");
    }
}
