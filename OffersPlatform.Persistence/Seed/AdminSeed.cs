using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Features.Auth.Commands.Register.RegisterUser;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;
namespace OffersPlatform.Persistence.Seed;

public class AdminSeeder
{
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;

    public void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        Migrate(_context);
        SeedAdmin(_context);
    }

    public AdminSeeder(IMapper mapper, ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _mapper = mapper;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public void SeedAdmin(ApplicationDbContext context)
    {
        try
        {
            Console.WriteLine("Checking if Admin user exists...");

            if (!_context.Users.
                    Any(u => u.Role == UserRole.Admin))
            {
                Console.WriteLine("Admin user not found. Creating...");

                var adminUser = new RegisterUserCommand
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Username = "admin",
                    Email = "admin@admin.com",
                    Password = "Admin1",
                    PhoneNumber = "123456789"
                };

                var user = _mapper.Map<User>(adminUser);
                user.Role = UserRole.Admin;

                user.PasswordHash = _passwordHasher.HashPassword(adminUser.Password);

                context.Users.Add(user);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during seeding: {ex.Message}");
        }
    }

    private static void Migrate(ApplicationDbContext context)
    {
        context.Database.Migrate();
    }

}
