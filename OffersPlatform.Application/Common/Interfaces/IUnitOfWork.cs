using OffersPlatform.Application.Common.Interfaces.IRepositories;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOfferRepository OfferRepository { get; }
    IUserRepository UserRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IPurchaseRepository PurchaseRepository { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
