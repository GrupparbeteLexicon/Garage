using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkedVehicleConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkedCarID",
                table: "ParkingSpot",
                newName: "ParkedVehicleID");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_Blocked_ParkedCarID",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_Blocked_ParkedVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpot_ParkedVehicleID",
                table: "ParkingSpot",
                column: "ParkedVehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpot_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpot",
                column: "ParkedVehicleID",
                principalTable: "ParkedVehicle",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpot_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpot");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpot_ParkedVehicleID",
                table: "ParkingSpot");

            migrationBuilder.RenameColumn(
                name: "ParkedVehicleID",
                table: "ParkingSpot",
                newName: "ParkedCarID");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_Blocked_ParkedVehicleID",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_Blocked_ParkedCarID");
        }
    }
}
