using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class UpdateHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Hours",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Hours_UserId",
                table: "Hours",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_Users_UserId",
                table: "Hours",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_Users_UserId",
                table: "Hours");

            migrationBuilder.DropIndex(
                name: "IX_Hours_UserId",
                table: "Hours");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Hours",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
