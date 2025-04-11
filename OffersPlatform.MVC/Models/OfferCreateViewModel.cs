// Copyright (C) TBC Bank. All Rights Reserved.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OffersPlatform.MVC.Models;

public class OfferCreateViewModel
{
    [Required]
    [Display(Name = "Category")]
    public Guid CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Initial Quantity")]
    public int InitialQuantity { get; set; }

    [Display(Name = "Expires At")]
    [DataType(DataType.DateTime)]
    public DateTime ExpiresAt { get; set; }

    public IFormFile? ImageFile { get; set; }
    public List<SelectListItem>? Categories { get; set; }
}
