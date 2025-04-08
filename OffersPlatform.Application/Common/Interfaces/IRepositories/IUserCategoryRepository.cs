using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IUserCategoryRepository
{
    Task<IEnumerable<UserCategory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserCategory?> GetByUserIdAndCategoryIdAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default);
    Task<UserCategory?> AddCategoryToPreferenceAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken);
    Task <bool> RemoveCategoryFromPreferenceAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken);
}