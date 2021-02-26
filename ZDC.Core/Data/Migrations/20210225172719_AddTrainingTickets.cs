using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDC.Core.Migrations
{
    public partial class AddTrainingTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<int>(nullable: true),
                    TrainerId = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Facility = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    StudentComments = table.Column<string>(nullable: true),
                    TrainerComments = table.Column<string>(nullable: true),
                    NoShow = table.Column<bool>(nullable: false),
                    OtsRecommendation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingTickets_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingTickets_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTickets_StudentId",
                table: "TrainingTickets",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTickets_TrainerId",
                table: "TrainingTickets",
                column: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTickets");
        }
    }
}
