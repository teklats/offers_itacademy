namespace OffersPlatform.Application.DTOs;

public class UserCategoryDto
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

public class UserCategoryUpdateDto
{
    public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}