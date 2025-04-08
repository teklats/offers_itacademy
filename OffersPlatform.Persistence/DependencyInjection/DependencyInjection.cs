using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Mapping;
using OffersPlatform.Persistence.Context;
using OffersPlatform.Persistence.Repositories;
using OffersPlatform.Persistence.Seed;
using OffersPlatform.Persistence.UnitOfWorkService;

namespace OffersPlatform.Persistence.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserCategoryRepository, UserCategoryRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();


        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<AdminSeeder>();
        
        return services;
    }
}