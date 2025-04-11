using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; }
}

public class UserBalanceDto
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
}

public class UserUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UserPurchaseDto
{
    public Guid OfferId { get; set; }
    public int Quantity { get; set; }
    public PurchaseStatus Status { get; set; }

}

public class UserPreferredCategoriesDto
{
    public List<Guid> CategoryIds { get; set; } = new();
}
