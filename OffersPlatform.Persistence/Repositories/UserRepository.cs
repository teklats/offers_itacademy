using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<User>> GetAllActiveUsersAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => u.IsActive && u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<User?> GetActiveUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => u.IsActive && u.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<User?> GetActiveUserByEmailAsync(string? email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => u.IsActive && u.Email == email)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string?> GetPasswordHashAsync(string? email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => u.Email == email)
            .Select(u => u.PasswordHash)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        if (user == null) return;

        user.IsActive = false;
        _dbContext.Users.Update(user);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }
}
