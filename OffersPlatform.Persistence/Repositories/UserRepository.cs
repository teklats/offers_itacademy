using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<User>> GetAllActiveUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<User?> GetActiveUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.IsActive && u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
    
    public async Task<User?> GetActiveUserByEmailAsync(string? email, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.IsActive && u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
        
        return user;
    }

    public async Task<string> GetPasswordHashAsync(string? email, CancellationToken cancellationToken)
    {
        var passwordHash = await _dbContext.Users
            .Where(u => u.Email == email)
            .Select(u => u.PasswordHash)
            .FirstOrDefaultAsync(cancellationToken);
        return passwordHash;
    }

    public async Task<User?> UpdateUserBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken)
    {
        var user = await GetActiveUserByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return null;
        }
        user.Balance += balance;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        if (user is null) return;

        user.IsActive = false;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}