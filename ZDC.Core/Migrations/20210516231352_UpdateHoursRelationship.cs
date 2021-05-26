using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class UpdateHoursRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Approach",
                table: "Certification",
                newName: "Shenandoah");

            migrationBuilder.AddColumn<int>(
                name: "Chesapeake",
                table: "Certification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinorApproach",
                table: "Certification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MountVernon",
                table: "Certification",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chesapeake",
                table: "Certification");

            migrationBuilder.DropColumn(
                name: "MinorApproach",
                table: "Certification");

            migrationBuilder.DropColumn(
                name: "MountVernon",
                table: "Certification");

            migrationBuilder.RenameColumn(
                name: "Shenandoah",
                table: "Certification",
                newName: "Approach");
        }
    }
}
