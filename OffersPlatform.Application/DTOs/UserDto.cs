using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public decimal? Balance { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; }
}