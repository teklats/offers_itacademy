using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Company;
    public CompanyStatus Status { get; set; }
}
