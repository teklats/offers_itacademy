using OffersPlatform.Application.Common.Interfaces;

namespace OffersPlatform.Infrastructure.PasswordService;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string? password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string? hash, string? password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
