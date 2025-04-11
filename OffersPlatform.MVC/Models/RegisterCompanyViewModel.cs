// Copyright (C) TBC Bank. All Rights Reserved.

using System.ComponentModel.DataAnnotations;

namespace OffersPlatform.MVC.Models;

public class RegisterCompanyViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }
}
