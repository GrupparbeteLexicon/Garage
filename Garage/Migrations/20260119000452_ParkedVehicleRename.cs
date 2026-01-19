using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkedVehicleRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_AspNetUsers_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkedVehicle",
                table: "ParkedVehicle");

            migrationBuilder.RenameTable(
                name: "ParkedVehicle",
                newName: "Vehicle");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "Vehicle",
                newName: "IX_Vehicle_VehicleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_Registration",
                table: "Vehicle",
                newName: "IX_Vehicle_Registration");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_ParkingSpotId",
                table: "Vehicle",
                newName: "IX_Vehicle_ParkingSpotId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "Vehicle",
                newName: "IX_Vehicle_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_AspNetUsers_OwnerId",
                table: "Vehicle",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_ParkingSpots_ParkingSpotId",
                table: "Vehicle",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_AspNetUsers_OwnerId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_ParkingSpots_ParkingSpotId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "ParkedVehicle");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_VehicleTypeId",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_VehicleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_Registration",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_Registration");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_ParkingSpotId",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_ParkingSpotId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_OwnerId",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkedVehicle",
                table: "ParkedVehicle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_AspNetUsers_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicle",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
