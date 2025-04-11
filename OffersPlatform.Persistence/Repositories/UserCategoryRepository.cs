using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class UserCategoryRepository : Repository<UserCategory>, IUserCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<UserCategory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserCategories
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Category)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<UserCategory?> GetByUserIdAndCategoryIdAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserCategories
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CategoryId == categoryId, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<UserCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserCategories
            .Include(uc => uc.User)
            .Include(uc => uc.Category)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public Task<UserCategory?> AddCategoryToPreferenceAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken)
    {
        var userCategory = new UserCategory
        {
            UserId = userId,
            CategoryId = categoryId,
            CreatedAt = DateTime.Now
        };

        _dbContext.UserCategories.Add(userCategory);
        return Task.FromResult<UserCategory?>(userCategory);
    }

    public async Task<bool> RemoveCategoryFromPreferenceAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken)
    {
        var userPreference = await _dbContext.UserCategories
            .FirstOrDefaultAsync(up => up.UserId == userId && up.CategoryId == categoryId, cancellationToken)
            .ConfigureAwait(false);

        if (userPreference != null)
        {
            _dbContext.UserCategories.Remove(userPreference);
            return true;
        }

        return false;
    }
}
