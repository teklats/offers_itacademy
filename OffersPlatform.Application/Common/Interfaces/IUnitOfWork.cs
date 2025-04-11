using OffersPlatform.Application.Common.Interfaces.IRepositories;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IOfferRepository OfferRepository { get; }
    IPurchaseRepository PurchaseRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IUserCategoryRepository UserCategoryRepository { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}

