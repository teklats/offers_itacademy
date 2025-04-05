namespace OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Domain.Entities;

public interface IJwtService
{
    string GenerateToken(User user);
}