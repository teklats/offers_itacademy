namespace OffersPlatform.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Offer>? Offers { get; set; }
    public ICollection<UserCategory>? UserCategories { get; set; }
}