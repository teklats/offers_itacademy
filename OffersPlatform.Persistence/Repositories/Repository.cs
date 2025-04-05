using OffersPlatform.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;

using AutoMapper; 

namespace OffersPlatform.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public Repository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _mapper = mapper; // Inject AutoMapper
        }

        public async Task<TDto?> GetByIdAsync<TDto>(Guid id, CancellationToken cancellationToken) where TDto : class
        {
            var entity = await _dbSet.FindAsync(new object[] {id}, cancellationToken);
            if (entity == null) return null;

            // Use AutoMapper to map entity to DTO
            return _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto?>> GetAllAsync<TDto>(CancellationToken cancellationToken) where TDto : class
        {
            var entities = await _dbSet.ToListAsync(cancellationToken);

            // Use AutoMapper to map entities to DTOs
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}