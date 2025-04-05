using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserRole Role { get; set; } = UserRole.Company;
    public CompanyStatus Status { get; set; }
}