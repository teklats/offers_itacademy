using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Domain.Entities;

public class Offer
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int InitialQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public OfferStatus Status { get; set; }

    // Foreign keys
    public int CompanyId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public Company? Company { get; set; }
    public Category? Category { get; set; }
    public ICollection<Purchase>? Purchases { get; set; }
}