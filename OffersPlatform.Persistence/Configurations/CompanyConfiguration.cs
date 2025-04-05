using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Define the primary key
            builder.HasKey(x => x.Id);

            // Configure properties with validation and constraints
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();  // Name is required

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();  // Email is required

            builder.Property(x => x.PasswordHash)
                .IsRequired();  // PasswordHash is required

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(1000);  // Optional field for image URL

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .HasDefaultValue((int)CompanyStatus.Inactive);

            builder.Property(x => x.CreatedAt)
                .IsRequired();  // CreatedAt is required and defaults to the current date/time

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);  // UpdatedAt is optional

            builder.Property(x => x.Role)
                .IsRequired()
                .HasConversion<string>();  // Enums are typically converted to string or integer in DB

            // Configure relationships (e.g., with Offers)
            builder.HasMany(c => c.Offers)
                .WithOne(o => o.Company)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when a company is deleted
        }
    }
}