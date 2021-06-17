using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ZDC.Core.Migrations
{
    public partial class RemoveMetar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Metar_MetarId",
                table: "Airports");

            migrationBuilder.DropTable(
                name: "Metar");

            migrationBuilder.DropIndex(
                name: "IX_Airports_MetarId",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "MetarId",
                table: "Airports");

            migrationBuilder.AddColumn<string>(
                name: "Altimeter",
                table: "Airports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "Airports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetarRaw",
                table: "Airports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp",
                table: "Airports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wind",
                table: "Airports",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altimeter",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "MetarRaw",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "Wind",
                table: "Airports");

            migrationBuilder.AddColumn<int>(
                name: "MetarId",
                table: "Airports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Metar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Altimeter = table.Column<string>(type: "text", nullable: true),
                    Conditions = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MetarRaw = table.Column<string>(type: "text", nullable: true),
                    Temp = table.Column<string>(type: "text", nullable: true),
                    Wind = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_MetarId",
                table: "Airports",
                column: "MetarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Metar_MetarId",
                table: "Airports",
                column: "MetarId",
                principalTable: "Metar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
