using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Features.Users.Commands.CreateUser;
using OffersPlatform.Infrastructure;
using OffersPlatform.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)); // Or typeof(SomeHandler)

// Register MediatR with the correct assembly
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(
        typeof(CreateUserCommandHandler).Assembly));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "API is running!");
app.Run();