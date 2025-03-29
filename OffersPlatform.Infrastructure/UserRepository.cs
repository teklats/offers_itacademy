using OffersPlatform.Domain.Entities;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}