using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSpotOwnsVehicleParked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID",
                unique: true,
                filter: "[ParkedVehicleID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID",
                principalTable: "ParkedVehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpots");

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotId",
                table: "ParkedVehicle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_ParkingSpotId",
                table: "ParkedVehicle",
                column: "ParkingSpotId",
                unique: true,
                filter: "[ParkingSpotId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicle",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
