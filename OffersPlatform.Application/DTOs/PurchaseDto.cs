using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.DTOs;

public class CancelPurchaseDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public Guid OfferId { get; set; }
    
}

public class PurchaseDto
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchasedAt { get; set; }
    public PurchaseStatus Status { get; set; }
    public Guid UserId { get; set; }
    public Guid OfferId { get; set; }
}