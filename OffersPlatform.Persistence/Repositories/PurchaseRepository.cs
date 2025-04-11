using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly ApplicationDbContext _context;

    public PurchaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Purchase purchase, CancellationToken cancellationToken)
    {
        await _context.Purchases.AddAsync(purchase, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Purchase?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .Include(p => p.Offer)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Purchase>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .Where(p => p.UserId == userId)
            .Include(p => p.Offer)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Purchase>> GetByOfferIdAsync(Guid offerId, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .Where(p => p.OfferId == offerId && p.Status == PurchaseStatus.Completed)
            .Include(p => p.User)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<Purchase>> GetByOfferIdsAsync(List<Guid> offerIds, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .Where(p => offerIds.Contains(p.OfferId))
            .Include(p => p.User)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }


    public async Task<IEnumerable<Guid>> GetUserByPurchaseId(Guid purchaseId, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .Where(p => p.Id == purchaseId)
            .Select(p => p.UserId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public Task DeleteAsync(Purchase purchase, CancellationToken cancellationToken)
    {
        _context.Purchases.Remove(purchase);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Purchase purchase, CancellationToken cancellationToken)
    {
        _context.Purchases.Update(purchase);
        return Task.CompletedTask;
    }
}
