// Copyright (C) TBC Bank. All Rights Reserved.

using OffersPlatform.Application.DTOs;

namespace OffersPlatform.MVC.Models;

public class UserDashboardViewModel
{
    public UserDto User { get; set; }
    public List<CategoryDto> SelectedCategories { get; set; } = new();
    public List<CategoryDto> AllCategories { get; set; } = new();
    public List<OfferDto> Offers { get; set; } = new();

    public List<PurchaseDto> Purchases { get; set; } = new();

    public decimal? BalanceToAdd { get; set; }
    public Guid CategoryIdToAdd { get; set; }
}
