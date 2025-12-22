using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerSocialSecurityNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PickupDateTime)
            .IsRequired();

        builder.Property(x => x.PickupMeterReading)
            .IsRequired();

        builder.Property(x => x.ReturnDateTime);

        builder.Property(x => x.ReturnMeterReading);

        builder.Property(x => x.CalculatedPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IsReturned)
            .IsRequired();

        builder.Property(x => x.CarId)
            .IsRequired();

        builder.HasOne(x => x.Car)
            .WithMany()
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}



