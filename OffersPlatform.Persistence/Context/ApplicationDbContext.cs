using Microsoft.EntityFrameworkCore;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
namespace OffersPlatform.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<UserCategory> UserCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entities
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasConversion<string>();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e=> e.Balance).IsRequired().HasDefaultValue(0);

            entity.HasMany(e => e.Purchases)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.PreferredCategories)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
            entity.Property(c => c.PasswordHash).IsRequired();
            entity.Property(c => c.Status)
                .HasConversion<int>() // Ensure that the enum is converted to int in the DB
                .HasDefaultValue(CompanyStatus.Inactive);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.ImageUrl).HasMaxLength(500);
            entity.Property(c => c.Balance).IsRequired().HasDefaultValue(0);
   
            entity.Property(c => c.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserRole.Company);
            
            entity
                .HasMany(c => c.Offers)
                .WithOne(o => o.Company)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
        
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.InitialQuantity).IsRequired();
            entity.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ExpiresAt).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Status).IsRequired()
                  .HasDefaultValue(OfferStatus.Active)
                  .HasConversion<string>();

            entity.HasOne(e => e.Company)
                  .WithMany(c => c.Offers)
                  .HasForeignKey(e => e.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Category)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PurchasedAt).IsRequired();
            entity.Property(e => e.Status).IsRequired()
                  .HasConversion<string>();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Purchases)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Offer)
                  .WithMany(o => o.Purchases)
                  .HasForeignKey(e => e.OfferId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserCategory>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CategoryId });

            entity.HasOne(up => up.User)
                .WithMany(u => u.PreferredCategories)
                .HasForeignKey(up => up.UserId);

            entity.HasOne(up => up.Category)
                .WithMany(c => c.UserCategories)
                .HasForeignKey(up => up.CategoryId);
        });
    }
}