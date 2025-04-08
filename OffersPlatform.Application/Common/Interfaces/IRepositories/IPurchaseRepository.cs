namespace OffersPlatform.Application.Common.Interfaces.IRepositories;
using Domain.Entities;

public interface IPurchaseRepository
{
    Task AddAsync(Purchase purchase, CancellationToken cancellationToken);
    Task<Purchase?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Purchase>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<Purchase>> GetByOfferIdAsync(Guid companyId, CancellationToken cancellationToken);
    Task<IEnumerable<Guid>> GetUserByPurchaseId(Guid purchaseId, CancellationToken cancellationToken);
    Task DeleteAsync(Purchase purchase, CancellationToken cancellationToken);
    Task UpdateAsync(Purchase purchase, CancellationToken cancellationToken);
}
