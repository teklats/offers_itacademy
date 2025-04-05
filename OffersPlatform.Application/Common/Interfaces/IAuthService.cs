using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IAuthService
{
    Task<string> RegisterUserAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<string> RegisterCompanyAsync(Company company, string password, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(string? email, string? password, CancellationToken cancellationToken = default);
}