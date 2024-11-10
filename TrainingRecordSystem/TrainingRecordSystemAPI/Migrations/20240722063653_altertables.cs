using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class altertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participates_Topics_TopicId",
                table: "Participates");

            migrationBuilder.DropIndex(
                name: "IX_Participates_TopicId",
                table: "Participates");

            migrationBuilder.DropColumn(
                name: "IsNominated",
                table: "Participates");

            migrationBuilder.DropColumn(
                name: "ModePreference",
                table: "Participates");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Participates");

            migrationBuilder.CreateTable(
                name: "Nominations",
                columns: table => new
                {
                    NominationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModePreference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    ParticipateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominations", x => x.NominationId);
                    table.ForeignKey(
                        name: "FK_Nominations_Participates_ParticipateId",
                        column: x => x.ParticipateId,
                        principalTable: "Participates",
                        principalColumn: "ParticipateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nominations_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId");
                    table.ForeignKey(
                        name: "FK_Nominations_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_ParticipateId",
                table: "Nominations",
                column: "ParticipateId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_TopicId",
                table: "Nominations",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_TrainerId",
                table: "Nominations",
                column: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nominations");

            migrationBuilder.AddColumn<bool>(
                name: "IsNominated",
                table: "Participates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModePreference",
                table: "Participates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Participates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participates_TopicId",
                table: "Participates",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participates_Topics_TopicId",
                table: "Participates",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId");
        }
    }
}
