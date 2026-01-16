using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSpotRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpot_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot");

            migrationBuilder.RenameTable(
                name: "ParkingSpot",
                newName: "ParkingSpots");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_ParkTime",
                table: "ParkingSpots",
                newName: "IX_ParkingSpots_ParkTime");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_ParkedVehicleID",
                table: "ParkingSpots",
                newName: "IX_ParkingSpots_ParkedVehicleID");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_Blocked_ParkedVehicleID",
                table: "ParkingSpots",
                newName: "IX_ParkingSpots_Blocked_ParkedVehicleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID",
                principalTable: "ParkedVehicle",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "ParkingSpots",
                newName: "ParkingSpot");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpots_ParkTime",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_ParkTime");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_ParkedVehicleID");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpots_Blocked_ParkedVehicleID",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_Blocked_ParkedVehicleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpot_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpot",
                column: "ParkedVehicleID",
                principalTable: "ParkedVehicle",
                principalColumn: "Id");
        }
    }
}
