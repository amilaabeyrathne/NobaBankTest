using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveReservationUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateIndex(
           name: "UX_Reservations_ActiveCar",
           table: "Reservations",
           column: "CarId",
           unique: true,
           filter: "IsReturned = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DropIndex(
           name: "UX_Reservations_ActiveCar",
           table: "Reservations");
        }
    }
}
