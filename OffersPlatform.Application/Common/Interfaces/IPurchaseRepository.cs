using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IPurchaseRepository
{
    Task<Purchase?> GetByIdAsync(Guid id);
    Task<IEnumerable<Purchase>> GetUserPurchasesAsync(Guid userId);
    Task<IEnumerable<Purchase>> GetCompanySalesAsync(Guid companyId);
    Task AddAsync(Purchase purchase);
    Task CancelPurchaseAsync(Guid purchaseId);
}