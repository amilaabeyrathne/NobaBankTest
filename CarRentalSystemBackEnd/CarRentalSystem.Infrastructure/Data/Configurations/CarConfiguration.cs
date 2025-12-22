using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        builder.Property(x => x.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Colour)
            .HasMaxLength(50);

        builder.Property(x => x.Milage)
            .IsRequired();

        builder.Property(x => x.IsAvailableToRent)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.HasOne<CarCategory>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}



