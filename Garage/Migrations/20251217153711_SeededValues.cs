using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class SeededValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "Brand", "Color", "Model", "ParkTime", "Registration", "VehicleType", "Wheels" },
                values: new object[,]
                {
                    { 1, "Volvo", "Red", "XC60", new DateTime(2025, 12, 17, 9, 30, 0, 0, DateTimeKind.Unspecified), "ABC123", 0, 4 },
                    { 2, "Yamaha", "Black", "MT-07", new DateTime(2025, 12, 17, 10, 15, 0, 0, DateTimeKind.Unspecified), "MOTO77", 1, 2 },
                    { 3, "Polaris", "Green", "Sportsman", new DateTime(2025, 12, 17, 11, 0, 0, 0, DateTimeKind.Unspecified), "ATV999", 2, 4 },
                    { 4, "Scania", "White", "Citywide", new DateTime(2025, 12, 17, 8, 45, 0, 0, DateTimeKind.Unspecified), "BUS001", 3, 6 },
                    { 5, "MAN", "Blue", "TGX", new DateTime(2025, 12, 17, 7, 20, 0, 0, DateTimeKind.Unspecified), "TRK888", 4, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
