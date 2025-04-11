using Microsoft.EntityFrameworkCore.Storage;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.Repositories;

namespace OffersPlatform.Persistence.UnitOfWorkService;
using Microsoft.EntityFrameworkCore.Storage;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Persistence.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository UserRepository { get; }
    public IOfferRepository OfferRepository { get; }
    public IPurchaseRepository PurchaseRepository { get; }
    public ICompanyRepository CompanyRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IUserCategoryRepository UserCategoryRepository { get; }

    private IDbContextTransaction? _transaction;


    public UnitOfWork(
        ApplicationDbContext context,
        IUserRepository userRepository,
        IOfferRepository offerRepository,
        IPurchaseRepository purchaseRepository,
        ICompanyRepository companyRepository,
        ICategoryRepository categoryRepository,
        IUserCategoryRepository userCategoryRepository)
    {
        _context = context;
        UserRepository = userRepository;
        OfferRepository = offerRepository;
        PurchaseRepository = purchaseRepository;
        CompanyRepository = companyRepository;
        CategoryRepository = categoryRepository;
        UserCategoryRepository = userCategoryRepository;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null) return;
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }
}
