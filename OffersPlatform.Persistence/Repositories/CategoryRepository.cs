using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CategoryRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

  
    public async Task<List<Category>> GetByIdsAsync(List<Guid> categoryIds, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}