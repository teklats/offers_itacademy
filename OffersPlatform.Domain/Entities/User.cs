using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal Balance { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }



    // Navigation Properties
    public IEnumerable<UserCategory>? PreferredCategories { get; set; }
    public IEnumerable<Purchase>? Purchases { get; set; }
}
