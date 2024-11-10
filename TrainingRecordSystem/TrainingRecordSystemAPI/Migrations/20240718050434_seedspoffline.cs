using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class seedspoffline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetModeofTrainingByTopicId
	                 @TopicId INT,
	                 @UserId INT
 
                AS
                BEGIN
 
	
	                SELECT tpd.ModePreference  
		                FROM TrainerProgramDetails tpd JOIN TrainerTopics tt ON tpd.TrainerTopicId = tt.TrainerTopicId
			                WHERE tt.TopicId = @TopicId AND tt.UserId = @UserId

 
                        END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetModeofTrainingByTopicId");
        }
    }
}
