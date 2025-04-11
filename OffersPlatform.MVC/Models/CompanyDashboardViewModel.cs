// Copyright (C) TBC Bank. All Rights Reserved.

using OffersPlatform.Application.DTOs;

namespace OffersPlatform.MVC.Models;

public class CompanyDashboardViewModel
{
    public CompanyDto Company { get; set; }
    public List<OfferDto> Offers { get; set; }
    public List<PurchaseDto> Purchases { get; set; }
}
