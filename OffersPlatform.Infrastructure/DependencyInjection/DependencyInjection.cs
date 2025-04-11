using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Infrastructure.Auth;
using OffersPlatform.Infrastructure.PasswordService;

namespace OffersPlatform.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Authentication Services
        services.AddScoped<IAuthService, JwtAuthService>();

        // Register Password Hasher
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
