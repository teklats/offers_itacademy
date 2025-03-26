using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.DTOs;

public class CompanyCreateDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}


public class CompanyDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<Offer>? Offers { get; set; }
}

public class CompanyUpdateDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public ICollection<Offer>? Offers { get; set; }
}