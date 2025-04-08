using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Domain.Entities;

public class Company
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserRole Role { get; set; } = UserRole.Company;
    public CompanyStatus Status { get; set; } = CompanyStatus.Inactive;

    // Navigation property
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}

