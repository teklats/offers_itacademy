namespace OffersPlatform.Application.DTOs;

public class UserCategoryDto
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime SubscribedAt { get; set; }
}

public class UserCategoryUpdateDto
{
    public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}

public class PreferredCategoryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public bool IsSubscribed { get; set; }
}
