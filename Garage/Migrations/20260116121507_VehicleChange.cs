using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class VehicleChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpots");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_Registration",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "ParkTime",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "ParkedVehicle");

            migrationBuilder.RenameColumn(
                name: "Wheels",
                table: "ParkedVehicle",
                newName: "VehicleTypeId");

            migrationBuilder.AddColumn<int>(
                name: "VehicleSize",
                table: "VehicleType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ParkedVehicle",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotId",
                table: "ParkedVehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PersonalID",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_ParkingSpotId",
                table: "ParkedVehicle",
                column: "ParkingSpotId",
                unique: true,
                filter: "[ParkingSpotId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_Registration",
                table: "ParkedVehicle",
                column: "Registration");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "ParkedVehicle",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FirstName_LastName",
                table: "AspNetUsers",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonalID",
                table: "AspNetUsers",
                column: "PersonalID",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_Registration",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FirstName_LastName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PersonalID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VehicleSize",
                table: "VehicleType");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "ParkingSpotId",
                table: "ParkedVehicle");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "ParkedVehicle",
                newName: "Wheels");

            migrationBuilder.AddColumn<DateTime>(
                name: "ParkTime",
                table: "ParkedVehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "ParkedVehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PersonalID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_Registration",
                table: "ParkedVehicle",
                column: "Registration",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_ParkedVehicle_ParkedVehicleID",
                table: "ParkingSpots",
                column: "ParkedVehicleID",
                principalTable: "ParkedVehicle",
                principalColumn: "Id");
        }
    }
}
