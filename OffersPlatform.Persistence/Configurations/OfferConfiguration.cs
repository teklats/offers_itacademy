using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Persistence.Configurations;
public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.Description)
            .HasMaxLength(500);

        builder.Property(o => o.InitialQuantity)
            .IsRequired();

        builder.Property(o => o.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.ExpiresAt)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Active")
            .IsRequired();

        // Ensure there is no duplicate index or foreign key
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.ExpiresAt);
        builder.HasIndex(o => new { o.CategoryId, o.Status });

        // Relationship configurations
        builder.HasOne(o => o.Category)
            .WithMany(c => c.Offers) // Ensure Category has one-to-many relationship
            .HasForeignKey(o => o.CategoryId) // Foreign key is CategoryId
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Purchases)
            .WithOne(p => p.Offer)
            .HasForeignKey(p => p.OfferId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}