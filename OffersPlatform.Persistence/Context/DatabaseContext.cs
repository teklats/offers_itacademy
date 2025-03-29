using Microsoft.EntityFrameworkCore;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Persistence.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; } // Add this DbSet
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure UserCategory primary key
            modelBuilder.Entity<UserCategory>(entity =>
            {
                entity.HasKey(uc => uc.UserId); // Or your preferred key
                
                // Example relationship configuration (if needed)
                entity.HasOne(uc => uc.User)
                    .WithMany(u => u.SubscribedCategories)
                    .HasForeignKey(uc => uc.UserId);
            });
            
            // Configure other entities...
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                // Additional user configurations...
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}