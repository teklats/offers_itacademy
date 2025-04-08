using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

public class Offer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int InitialQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public OfferStatus Status { get; set; }

    public Guid CompanyId { get; set; }
    public Guid CategoryId { get; set; }

    // Navigation property to the Category
    public Category Category { get; set; }

    // Navigation property to the Company
    public Company Company { get; set; }

    // Purchases collection
    public ICollection<Purchase> Purchases { get; set; }
}