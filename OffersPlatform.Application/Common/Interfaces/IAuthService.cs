using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IAuthService
{
    Task<string> RegisterUserAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<string> RegisterCompanyAsync(Company company, string password, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(UserRole role, Guid id, CancellationToken cancellationToken = default);
}