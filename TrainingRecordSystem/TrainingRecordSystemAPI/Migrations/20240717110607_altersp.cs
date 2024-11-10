using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class altersp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER   PROCEDURE [dbo].[UpcomingTrainingProgram]
	            @JobId INT=NULL
                AS
                BEGIN
        	    SELECT (u.FirstName +' '+ u.LastName) AS TrainerName
                ,t.TopicName
                 ,j.JobName,
			     tpd.StartDate,tpd.EndDate
                FROM TrainerTopics tt
                join Users u ON u.UserId=tt.UserId
                join Jobs j ON tt.JobId=j.JobId
                 join Topics t ON t.TopicId=tt.TopicId
                   join TrainerProgramDetails tpd On tpd.TrainerTopicId=tt.TrainerTopicId
                      where @JobId IS NULL and tpd.StartDate>=CURRENT_TIMESTAMP OR tt.JobId=@JobId and tpd.StartDate>=CURRENT_TIMESTAMP
                END
            GO");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpcomingTrainingProgram");
           
        }
    }
}
