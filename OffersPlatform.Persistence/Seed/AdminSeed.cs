using MediatR;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Features.Auth.UserAuth.Commands.RegisterUser;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Seed;

public class AdminSeeder
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public AdminSeeder(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            Console.WriteLine("Checking if Admin user exists...");

            if (!await _context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                Console.WriteLine("Admin user not found. Creating...");

                var adminUser = new RegisterUserCommand
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Username = "admin",
                    Email = "admin@admin.com",
                    Password = "admin"
                };

                var result = await _mediator.Send(adminUser);
                Console.WriteLine($"Admin creation result: {result}");
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
}