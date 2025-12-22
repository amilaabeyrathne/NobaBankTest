using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "CarCategories",
            columns: new[] { "Id", "Name", "BaseDayRental", "BaseKmPrice", "DayMultiplier", "KmMultiplier", "IsActive", "PricingStrategy" },
            values: new object[,]
            {
                   { 1, "Small Car",100.0m,10.0m, 1.0m, 0m, true,1},
                   { 2, "Combi",110.0m,12.0m, 1.3m, 1.0m, true,2 },
                   { 3, "Truck",130.0m,17.0m, 1.5m, 1.5m, true,3 }
             });

            migrationBuilder.InsertData(
               table: "Cars",
               columns: new[] { "Id", "RegistrationNumber", "CategoryId", "Milage", "Brand", "Model", "Colour", "IsAvailableToRent" },
               values: new object[,]
               {
                    // Small Cars (CategoryId = 1)
                    { new Guid("11111111-0000-0000-0000-000000000001"), "SC-1001", 1, 14500, "Toyota", "Yaris", "Red", true },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "SC-1002", 1, 23800, "Honda", "Jazz", "Blue", true },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "SC-1003", 1, 18200, "Ford", "Fiesta", "White", true },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "SC-1004", 1, 21000, "Hyundai", "i20", "Gray", true },
                    { new Guid("11111111-0000-0000-0000-000000000005"), "SC-1005", 1, 19500, "Kia", "Rio", "Black", true },
                    { new Guid("11111111-0000-0000-0000-000000000006"), "SC-1006", 1, 17250, "Nissan", "Micra", "Silver", true },
                    { new Guid("11111111-0000-0000-0000-000000000007"), "SC-1007", 1, 16000, "VW", "Polo", "White", true },
                    { new Guid("11111111-0000-0000-0000-000000000008"), "SC-1008", 1, 20500, "Peugeot", "208", "Red", true },
                    { new Guid("11111111-0000-0000-0000-000000000009"), "SC-1009", 1, 18750, "Renault", "Clio", "Blue", true },
                    { new Guid("11111111-0000-0000-0000-000000000010"), "SC-1010", 1, 19900, "Mazda", "2", "Gray", true },
                    { new Guid("11111111-0000-0000-0000-000000000011"), "SC-1011", 1, 22300, "Skoda", "Fabia", "Green", true },
                    { new Guid("11111111-0000-0000-0000-000000000012"), "SC-1012", 1, 17500, "Seat", "Ibiza", "Yellow", true },

                    // Combis (CategoryId = 2)
                    { new Guid("22222222-0000-0000-0000-000000000001"), "CB-2001", 2, 30500, "Toyota", "Corolla Touring", "White", true },
                    { new Guid("22222222-0000-0000-0000-000000000002"), "CB-2002", 2, 28900, "Honda", "Civic Tourer", "Blue", true },
                    { new Guid("22222222-0000-0000-0000-000000000003"), "CB-2003", 2, 31800, "Ford", "Focus Wagon", "Gray", true },
                    { new Guid("22222222-0000-0000-0000-000000000004"), "CB-2004", 2, 27600, "Hyundai", "i30 Wagon", "Silver", true },
                    { new Guid("22222222-0000-0000-0000-000000000005"), "CB-2005", 2, 30100, "Kia", "Ceed SW", "Red", true },
                    { new Guid("22222222-0000-0000-0000-000000000006"), "CB-2006", 2, 33000, "VW", "Golf Variant", "Black", true },
                    { new Guid("22222222-0000-0000-0000-000000000007"), "CB-2007", 2, 29500, "Skoda", "Octavia Combi", "White", true },
                    { new Guid("22222222-0000-0000-0000-000000000008"), "CB-2008", 2, 28400, "Peugeot", "308 SW", "Blue", true },
                    { new Guid("22222222-0000-0000-0000-000000000009"), "CB-2009", 2, 31000, "Renault", "Megane Sport Tourer", "Gray", true },

                    // Trucks (CategoryId = 3)
                    { new Guid("33333333-0000-0000-0000-000000000001"), "TR-3001", 3, 52000, "Ford", "Transit", "White", true },
                    { new Guid("33333333-0000-0000-0000-000000000002"), "TR-3002", 3, 56500, "Mercedes", "Sprinter", "Silver", true },
                    { new Guid("33333333-0000-0000-0000-000000000003"), "TR-3003", 3, 54800, "VW", "Crafter", "Blue", true },
                    { new Guid("33333333-0000-0000-0000-000000000004"), "TR-3004", 3, 53300, "Renault", "Master", "Gray", true },
                    { new Guid("33333333-0000-0000-0000-000000000005"), "TR-3005", 3, 55700, "Fiat", "Ducato", "Red", true }
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var carIds = new Guid[]
            {
                new Guid("11111111-0000-0000-0000-000000000001"),
                new Guid("11111111-0000-0000-0000-000000000002"),
                new Guid("11111111-0000-0000-0000-000000000003"),
                new Guid("11111111-0000-0000-0000-000000000004"),
                new Guid("11111111-0000-0000-0000-000000000005"),
                new Guid("11111111-0000-0000-0000-000000000006"),
                new Guid("11111111-0000-0000-0000-000000000007"),
                new Guid("11111111-0000-0000-0000-000000000008"),
                new Guid("11111111-0000-0000-0000-000000000009"),
                new Guid("11111111-0000-0000-0000-000000000010"),
                new Guid("11111111-0000-0000-0000-000000000011"),
                new Guid("11111111-0000-0000-0000-000000000012"),
                new Guid("22222222-0000-0000-0000-000000000001"),
                new Guid("22222222-0000-0000-0000-000000000002"),
                new Guid("22222222-0000-0000-0000-000000000003"),
                new Guid("22222222-0000-0000-0000-000000000004"),
                new Guid("22222222-0000-0000-0000-000000000005"),
                new Guid("22222222-0000-0000-0000-000000000006"),
                new Guid("22222222-0000-0000-0000-000000000007"),
                new Guid("22222222-0000-0000-0000-000000000008"),
                new Guid("22222222-0000-0000-0000-000000000009"),
                new Guid("22222222-0000-0000-0000-000000000010"),
                new Guid("33333333-0000-0000-0000-000000000001"),
                new Guid("33333333-0000-0000-0000-000000000002"),
                new Guid("33333333-0000-0000-0000-000000000003"),
                new Guid("33333333-0000-0000-0000-000000000004"),
                new Guid("33333333-0000-0000-0000-000000000005")
            };

            foreach (var id in carIds)
            {
                migrationBuilder.DeleteData(
                    table: "Cars",
                    keyColumn: "Id",
                    keyValue: id);
            }

            var categoryIds = new int[] { 1, 2, 3 };

            foreach (var id in categoryIds)
            {
                migrationBuilder.DeleteData(
                    table: "CarCategories",
                    keyColumn: "Id",
                    keyValue: id);
            }
        }
    }
}
