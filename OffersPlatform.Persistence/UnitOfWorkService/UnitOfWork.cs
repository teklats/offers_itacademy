using Microsoft.EntityFrameworkCore.Storage;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.Repositories;

namespace OffersPlatform.Persistence.UnitOfWorkService;

 public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public IOfferRepository OfferRepository { get; }
    public IUserRepository UserRepository { get; }
    public IPurchaseRepository PurchaseRepository { get; }
    public ICompanyRepository CompanyRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        // Initialize repositories
        OfferRepository = new OfferRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        PurchaseRepository = new PurchaseRepository(_dbContext);
        CompanyRepository = new CompanyRepository(_dbContext);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _dbContext.Database.
            BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.
                SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            _transaction?.
                CommitAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.
                    DisposeAsync()
                    .ConfigureAwait(false);
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _transaction?.RollbackAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.
                    DisposeAsync()
                    .ConfigureAwait(false);
                _transaction = null;
            }
        }
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.
            SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _dbContext.Dispose();
            }
            _disposed = true;
        }
    }
}
