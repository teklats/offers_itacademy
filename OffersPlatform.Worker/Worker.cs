
using OffersPlatform.Application.Common.Interfaces.IRepositories;

namespace OffersPlatform.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Offer archiving worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var offerRepo = scope.ServiceProvider.GetRequiredService<IOfferRepository>();
                        _logger.LogInformation("Checking for expired offers at: {time}", DateTimeOffset.Now);
                        await offerRepo.ArchiveExpiredOffersAsync(stoppingToken)
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error archiving expired offers");
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken)
                    .ConfigureAwait(false);
            }

            _logger.LogInformation("Offer archiving worker stopped at: {time}", DateTimeOffset.Now);
        }
    }
}
