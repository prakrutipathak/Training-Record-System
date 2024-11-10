using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class addtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Participates",
                columns: table => new
                {
                    ParticipateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModePreference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNominated = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participates", x => x.ParticipateId);
                    table.ForeignKey(
                        name: "FK_Participates_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participates_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId");
                    table.ForeignKey(
                        name: "FK_Participates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerProgramDetails",
                columns: table => new
                {
                    TrainerProgramDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ModePreference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainerTopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerProgramDetails", x => x.TrainerProgramDetailId);
                    table.ForeignKey(
                        name: "FK_TrainerProgramDetails_TrainerTopics_TrainerTopicId",
                        column: x => x.TrainerTopicId,
                        principalTable: "TrainerTopics",
                        principalColumn: "TrainerTopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobId",
                table: "Users",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Participates_JobId",
                table: "Participates",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Participates_TopicId",
                table: "Participates",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Participates_UserId",
                table: "Participates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerProgramDetails_TrainerTopicId",
                table: "TrainerProgramDetails",
                column: "TrainerTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Jobs_JobId",
                table: "Users",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Jobs_JobId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Participates");

            migrationBuilder.DropTable(
                name: "TrainerProgramDetails");

            migrationBuilder.DropIndex(
                name: "IX_Users_JobId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Users");
        }
    }
}
