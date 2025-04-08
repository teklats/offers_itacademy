namespace OffersPlatform.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation property to the related Offers
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public ICollection<UserCategory> UserCategories { get; set; }
}
