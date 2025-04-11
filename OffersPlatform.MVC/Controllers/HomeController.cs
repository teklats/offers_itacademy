// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Mvc;

namespace OffersPlatform.MVC.Controllers
{
    public class HomeController : Controller
    {
        // The Index action is typically the default page of the application
        public IActionResult Index()
        {
            // You can return a view or redirect to another action
            return View();
        }

        // About action - typically a page with information about your app
        public IActionResult About()
        {
            return View();
        }

        // Contact action - typically a page with a contact form or information
        public IActionResult Contact()
        {
            return View();
        }

        // Error action - typically a fallback for when an error occurs
        public IActionResult Error()
        {
            return View();
        }
    }
}
