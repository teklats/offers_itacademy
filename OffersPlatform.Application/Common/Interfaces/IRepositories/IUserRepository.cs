using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IUserRepository : IRepository<User>
{
        Task<User?> GetActiveUserByEmailAsync(string? email, CancellationToken cancellationToken = default);
        Task<string?> GetPasswordHashAsync(string? email, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllActiveUsersAsync(CancellationToken cancellationToken = default);
        Task<User?> GetActiveUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> UpdateUserBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
