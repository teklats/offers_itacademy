// Copyright (C) TBC Bank. All Rights Reserved.

using System.ComponentModel.DataAnnotations;


namespace OffersPlatform.MVC.Models;

public class RegisterUserViewModel
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Must contain at least one number, one upper case letter")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

}

