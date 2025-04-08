using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IOfferRepository
{
    Task<Offer?> GetByIdAsync(Guid offerId, CancellationToken cancellationToken);
    Task<Offer?> GetByIdAsync(Guid offerId, Guid companyId ,CancellationToken cancellationToken);
    Task<IEnumerable<Offer>> GetAllOffersAsync(Guid companyId, CancellationToken cancellationToken);
    Task<IEnumerable<Offer>> GetByCategoryIdsAsync(IEnumerable<Guid> categoryIds, CancellationToken cancellationToken);
    Task<IEnumerable<Offer>> GetCompanyOffersAsync(Guid companyId, CancellationToken cancellationToken);
    Task AddAsync(Offer offer, CancellationToken cancellationToken);
    Task UpdateAsync(Offer offer, CancellationToken cancellationToken);
    Task ArchiveExpiredOffersAsync(CancellationToken cancellationToken);
    Task<bool> CancelOfferAsync(Guid offerId, Guid companyId, CancellationToken cancellationToken);

}