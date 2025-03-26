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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public decimal? Balance { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public ICollection<Category>? SubscribedCategories { get; set; }
    public ICollection<Purchase>? Purchases { get; set; }
}


// public class User
// {
//
//     // Navigation properties
//     public ICollection<UserCategoryPreference> CategoryPreferences { get; set; }
//     public ICollection<Purchase> Purchases { get; set; }
// }