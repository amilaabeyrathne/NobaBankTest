using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations;

public class CarCategoryConfiguration : IEntityTypeConfiguration<CarCategory>
{
    public void Configure(EntityTypeBuilder<CarCategory> builder)
    {
        builder.ToTable("CarCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.DayMultiplier)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.KmMultiplier)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.BaseDayRental)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.BaseKmPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.PricingStrategy)
            .IsRequired();
    }
}

