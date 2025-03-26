namespace OffersPlatform.Application.DTOs;

public class CategoryCreateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}