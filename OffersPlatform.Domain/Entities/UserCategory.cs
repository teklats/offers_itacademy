namespace OffersPlatform.Domain.Entities;

public class UserCategory
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? User { get; set; }
    public Category? Category { get; set; }
}
