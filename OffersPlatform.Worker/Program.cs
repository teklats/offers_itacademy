using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.Repositories;
using OffersPlatform.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IOfferRepository, OfferRepository>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync().ConfigureAwait(false);
