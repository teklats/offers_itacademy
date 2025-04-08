using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Category?>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _dbContext.Categories
            .AddAsync(category, cancellationToken)
            .ConfigureAwait(false);

        await _dbContext.
            SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
