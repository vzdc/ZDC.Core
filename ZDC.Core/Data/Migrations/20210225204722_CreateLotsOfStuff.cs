using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class CreateLotsOfStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loas");

            migrationBuilder.DropTable(
                name: "Warnings");

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Icao = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Callsign = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Facility = table.Column<string>(nullable: true),
                    Service = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loa_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warning",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warning_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Loa_UserId",
                table: "Loa",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId",
                table: "Warning",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Loa");

            migrationBuilder.DropTable(
                name: "Warning");

            migrationBuilder.CreateTable(
                name: "Loas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MoreInfo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Reason = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warnings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warnings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loas_UserId",
                table: "Loas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_UserId",
                table: "Warnings",
                column: "UserId");
        }
    }
}
