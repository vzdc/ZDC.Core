using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ZDC.Core.Migrations
{
    public partial class BugReportsAndStaffingRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "BugReports",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: true),
                    DateTime = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    Description = table.Column<string>("text", nullable: true),
                    Status = table.Column<int>("integer", nullable: false),
                    Created = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>("timestamp without time zone", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_BugReports", x => x.Id); });

            migrationBuilder.CreateTable(
                "StaffingRequests",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: true),
                    Email = table.Column<string>("text", nullable: true),
                    Affiliation = table.Column<string>("text", nullable: true),
                    Description = table.Column<string>("text", nullable: true),
                    Date = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    Start = table.Column<TimeSpan>("interval", nullable: false),
                    End = table.Column<TimeSpan>("interval", nullable: false),
                    Status = table.Column<int>("integer", nullable: false),
                    Created = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>("timestamp without time zone", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_StaffingRequests", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "BugReports");

            migrationBuilder.DropTable(
                "StaffingRequests");
        }
    }
}