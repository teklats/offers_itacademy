using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IUserRepository : IRepository<User>
{
        Task<UserDto?> GetActiveUserByEmailAsync(string? email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllActiveUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> GetActiveUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}