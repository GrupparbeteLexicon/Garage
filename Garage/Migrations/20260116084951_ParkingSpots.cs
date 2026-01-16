using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSpots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkedVehicleID = table.Column<int>(type: "int", nullable: true),
                    ParkingSpotSize = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    ParkTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                        column: x => x.ParkedVehicleID,
                        principalTable: "ParkedVehicle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_Blocked_ParkedVehicleID",
                table: "ParkingSpots",
                columns: new[] { "Blocked", "ParkedVehicleID" });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_ParkTime",
                table: "ParkingSpots",
                column: "ParkTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSpots");

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
    }
}
