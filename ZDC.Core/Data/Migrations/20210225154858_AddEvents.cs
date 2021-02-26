using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class AddEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Banner = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Open = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    EventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_EventId",
                table: "EventRegistration",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRegistration");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
