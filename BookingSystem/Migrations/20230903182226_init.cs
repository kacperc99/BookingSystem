using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeskModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeskModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeskModel_LocationModel_LocationID",
                        column: x => x.LocationID,
                        principalTable: "LocationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingBeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookignEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationModel_DeskModel_DeskId",
                        column: x => x.DeskId,
                        principalTable: "DeskModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationModel_UserModel_UserId",
                        column: x => x.UserId,
                        principalTable: "UserModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeskModel_LocationID",
                table: "DeskModel",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationModel_DeskId",
                table: "ReservationModel",
                column: "DeskId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationModel_UserId",
                table: "ReservationModel",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationModel");

            migrationBuilder.DropTable(
                name: "DeskModel");

            migrationBuilder.DropTable(
                name: "UserModel");

            migrationBuilder.DropTable(
                name: "LocationModel");
        }
    }
}
