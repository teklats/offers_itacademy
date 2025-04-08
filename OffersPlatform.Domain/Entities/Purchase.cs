using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchasedAt { get; set; }
    public PurchaseStatus Status { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Foreign keys
    public Guid UserId { get; set; }
    public Guid OfferId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Offer? Offer { get; set; }
}
