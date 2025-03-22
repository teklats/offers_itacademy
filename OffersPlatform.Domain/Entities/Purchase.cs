using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Domain.Entities;

public class Purchase
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchasedAt { get; set; }
    public PurchaseStatus Status { get; set; }

    // Foreign keys
    public int UserId { get; set; }
    public int OfferId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Offer? Offer { get; set; }
}