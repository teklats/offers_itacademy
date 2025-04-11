using System.ComponentModel.DataAnnotations;
using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Auth.Commands.Register.RegisterCompany;


public class RegisterCompanyCommand : IRequest<AuthDto?>
{
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string? Address { get; set; }
}
