using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class UpdateSomeModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineControllers_Users_UserId",
                table: "OnlineControllers");

            migrationBuilder.DropIndex(
                name: "IX_OnlineControllers_UserId",
                table: "OnlineControllers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OnlineControllers");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Dossier");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Certification");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Certification");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OnlineControllers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Notifications",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OnlineControllers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OnlineControllers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Dossier",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Certification",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Certification",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_OnlineControllers_UserId",
                table: "OnlineControllers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineControllers_Users_UserId",
                table: "OnlineControllers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
