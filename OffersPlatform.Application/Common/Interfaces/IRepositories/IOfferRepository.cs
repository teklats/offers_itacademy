using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IOfferRepository
{
    Task<Offer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Offer>> GetActiveOffersAsync();
    Task<IEnumerable<Offer>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Offer>> GetCompanyOffersAsync(Guid companyId);
    Task AddAsync(Offer offer);
    Task UpdateAsync(Offer offer);
    Task ArchiveExpiredOffersAsync();
    Task CancelOfferAsync(Guid offerId);
}