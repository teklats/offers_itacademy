using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Category>> GetByIdsAsync(List<Guid> categoryIds, CancellationToken cancellationToken = default);
    // Task AddAsync(Category category, CancellationToken cancellationToken = default);
    // Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
    // Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
}