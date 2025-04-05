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
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllActiveUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetActiveUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.IsActive && u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<UserDto?>(user);
    }
    
    public async Task<UserDto?> GetActiveUserByEmailAsync(string? email, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.IsActive && u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
        
        return _mapper.Map<UserDto?>(user);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        if (user is null) return;

        user.IsActive = false;
        _dbContext.Users.Update(user);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
}