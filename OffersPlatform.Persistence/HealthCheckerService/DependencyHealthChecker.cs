using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.HealthCheckerService;

public class DependencyHealthChecker : IDependencyHealthChecker
{
    private readonly ApplicationDbContext _dbContext;

    public DependencyHealthChecker(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CanConnectToDatabaseAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database
            .CanConnectAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
