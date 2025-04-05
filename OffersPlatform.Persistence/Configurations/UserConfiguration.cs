using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();
       
        builder.Property(x => x.Email)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x=> x.UserName)
            .HasMaxLength(50).IsRequired();
        
        builder.Property(x=> x.PasswordHash)
            .IsRequired();
        
        builder.Property(x=> x.Role)
            .IsRequired();
        
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        builder.HasIndex(u => u.UserName)
            .IsUnique();
        
        builder.HasMany(u => u.Purchases)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}