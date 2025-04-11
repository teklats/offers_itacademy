// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OffersPlatform.MVC.Models;
public class CategoryPreferencesViewModel
{
    public List<Guid> SelectedCategoryIds { get; set; } = new();
    public List<SelectListItem> AllCategories { get; set; } = new();
}
