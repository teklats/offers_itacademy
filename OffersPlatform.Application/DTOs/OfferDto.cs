namespace OffersPlatform.Application.DTOs;

public class OfferCreateDto
{
    public Guid CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int InitialQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime ExpiresAt { get; set; }
}


public class OfferDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int InitialQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public Guid CompanyId { get; set; }
    public Guid CategoryId { get; set; }
}


public class OfferUpdateDto
{
    public Guid CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal DiscountedPrice { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime EndDate { get; set; }
    public string? Image { get; set; }
}