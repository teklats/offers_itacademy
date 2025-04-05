using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IUserCategoryRepository
{
    Task<List<UserCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    // Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<UserCategory>> GetByUserIdAsync(Guid userId);
    Task<UserCategory?> GetByUserIdAndCategoryIdAsync(Guid userId, Guid categoryId);
    Task AddAsync(UserCategory userCategory, CancellationToken cancellationToken = default);
    Task RemoveAsync(UserCategory userCategory, CancellationToken cancellationToken = default);
    // Task UpdateAsync(UserCategory userCategory, CancellationToken cancellationToken = default);
}