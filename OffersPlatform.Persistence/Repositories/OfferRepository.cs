using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace OffersPlatform.Persistence.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly ApplicationDbContext _context;

    public OfferRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Offers
            .Include(o => o.Company)
            .Include(o => o.Category)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Offer?> GetByIdAsync(Guid offerId, Guid companyId, CancellationToken cancellationToken)
    {
        return await _context.Offers
            .Where(o => o.Id == offerId && o.CompanyId == companyId)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Offer>> GetAllOffersAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return await _context.Offers
            .Where(o => o.CompanyId == companyId &&
                        (o.Status == OfferStatus.Active || o.Status == OfferStatus.Archived))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Offer>> GetByCategoryIdsAsync(IEnumerable<Guid> categoryIds, CancellationToken cancellationToken)
    {
        return await _context.Offers
            .Where(o => categoryIds.Contains(o.CategoryId) &&
                        o.ExpiresAt > DateTime.Now &&
                        o.Status == OfferStatus.Active)
            .Include(o => o.Company)
            .Include(o => o.Category)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Offer>> GetCompanyOffersAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return await _context.Offers
            .Where(o => o.CompanyId == companyId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public Task AddAsync(Offer offer, CancellationToken cancellationToken)
    {
        _context.Offers.Add(offer);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Offer offer, CancellationToken cancellationToken)
    {
        _context.Offers.Update(offer);
        return Task.CompletedTask;
    }

    public async Task ArchiveExpiredOffersAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var expiredOffers = await _context.Offers
            .Where(o => o.ExpiresAt < now && o.Status == OfferStatus.Active)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        foreach (var offer in expiredOffers)
        {
            offer.Status = OfferStatus.Archived;
        }
    }

    public async Task<bool> CancelOfferAsync(Guid offerId, Guid companyId, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers
            .FirstOrDefaultAsync(o => o.Id == offerId && o.CompanyId == companyId, cancellationToken)
            .ConfigureAwait(false);

        if (offer != null)
        {
            offer.Status = OfferStatus.Canceled;
            _context.Offers.Update(offer);
            return true;
        }

        return false;
    }
}
