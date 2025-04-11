using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.DTOs;

public class PurchaseHistoryDto
{
    public Guid Id { get; set; }
    public string OfferName { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class PurchaseDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchasedAt { get; set; }
    public PurchaseStatus Status { get; set; } // Status enum (Pending, Completed, etc.)
    public Guid UserId { get; set; }
    public Guid OfferId { get; set; }
    public string OfferName { get; set; } // Add OfferName for display
}
