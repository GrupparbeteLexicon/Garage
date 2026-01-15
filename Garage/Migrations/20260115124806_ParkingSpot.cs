using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSpot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSpot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkedCarID = table.Column<int>(type: "int", nullable: true),
                    ParkingSpotSize = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    ParkTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpot", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpot_Blocked_ParkedCarID",
                table: "ParkingSpot",
                columns: new[] { "Blocked", "ParkedCarID" });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpot_ParkTime",
                table: "ParkingSpot",
                column: "ParkTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSpot");
        }
    }
}
