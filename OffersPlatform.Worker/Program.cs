using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.DependencyInjection;
using OffersPlatform.Persistence.UnitOfWorkService;
using OffersPlatform.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddPersistenceServices(configuration);
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync().ConfigureAwait(false);
