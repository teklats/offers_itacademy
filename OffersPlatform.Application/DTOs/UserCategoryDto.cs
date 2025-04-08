namespace OffersPlatform.Application.DTOs;


public class UserCategoryUpdateDto
{
    public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}

public class UserCategoryDto
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
}

