using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    // Task UpdateUserAsync(User user);
    // Task DeleteUserAsync(Guid id);

}