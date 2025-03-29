using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.DTOs;

public class UserCreateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class DeleteUserDto
{
    public Guid Id { get; set; }
}