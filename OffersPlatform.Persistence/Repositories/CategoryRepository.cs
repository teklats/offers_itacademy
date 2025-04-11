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
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _dbContext.Categories
            .AddAsync(category, cancellationToken)
            .ConfigureAwait(false);
    }
}
