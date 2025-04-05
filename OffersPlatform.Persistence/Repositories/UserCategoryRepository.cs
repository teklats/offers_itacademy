using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class UserCategoryRepository : Repository<UserCategory>, IUserCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserCategoryRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }
    
    public async Task<List<UserCategory>> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserCategories
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Category)  // Include category for each user-category relationship
            .ToListAsync();
    }

    public async Task<UserCategory?> GetByUserIdAndCategoryIdAsync(Guid userId, Guid categoryId)
    {
        return await _dbContext.UserCategories
            .Where(uc => uc.UserId == userId && uc.CategoryId == categoryId)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(UserCategory userCategory, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserCategories.AddAsync(userCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(UserCategory userCategory, CancellationToken cancellationToken = default)
    {
        _dbContext.UserCategories.Remove(userCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<UserCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserCategories
            .Include(uc => uc.User)
            .Include(uc => uc.Category)
            .ToListAsync();
    }
}