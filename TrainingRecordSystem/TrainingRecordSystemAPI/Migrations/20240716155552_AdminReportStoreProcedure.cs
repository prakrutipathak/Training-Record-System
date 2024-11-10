using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class AdminReportStoreProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              CREATE OR ALTER PROCEDURE MonthlyAdminReport
					 @UserId INT,
					 @Month INT = NULL,
					 @Year INT = NULL
				 AS
				 BEGIN

					 SELECT  t.TopicName ,tpd.StartDate,tpd.EndDate,tpd.Duration,tpd.ModePreference, Count(n.NominationId) AS TotalParticipateNo 
						FROM Nominations n JOIN Topics t ON n.TopicId = t.TopicId
							  JOIN TrainerTopics tt ON n.TopicId = tt.TopicId
								  JOIN TrainerProgramDetails tpd ON tt.TrainerTopicId = tpd.TrainerTopicId
									JOIN Participates p ON n.ParticipateId = p.ParticipateId
										WHERE 
											(tt.UserId = @UserId AND @Month IS NULL AND @Year IS NULL)
											 OR (tt.UserId = @UserId AND Month(tpd.StartDate) = @Month AND Year(tpd.StartDate) = @Year)
												Group By t.TopicName,tpd.StartDate,tpd.EndDate,tpd.Duration,tpd.ModePreference

				END
            ");

            migrationBuilder.Sql(@"
              CREATE OR ALTER PROCEDURE DaterangeBasedReport
	             @JobId INT,
	             @StartDate DateTime = NULL,
	             @EndDate DateTime = NULL
             AS
             BEGIN

	
            SELECT  t.TopicName ,(u.FirstName +' '+ u.LastName) AS TrainerName,Count(n.NominationId) AS TotalParticipateNo ,tpd.StartDate ,tpd.EndDate,tpd.Duration,tpd.ModePreference
		            FROM Nominations n JOIN Topics t ON n.TopicId = t.TopicId
			            JOIN Participates p ON n.ParticipateId = p.ParticipateId
			              JOIN TrainerTopics tt ON n.TopicId = tt.TopicId
				              JOIN TrainerProgramDetails tpd ON tt.TrainerTopicId = tpd.TrainerTopicId
					            JOIN Users u ON u.UserId = tt.UserId
						            WHERE p.JobId = @JobId AND tpd.StartDate BETWEEN @StartDate AND @EndDate OR
						            p.JobId = @JobId AND @StartDate IS NULL AND @EndDate IS NULL
							            Group By t.TopicName,(u.FirstName +' '+ u.LastName),tpd.StartDate,tpd.EndDate,tpd.Duration,tpd.ModePreference

            END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MonthlyAdminReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DaterangeBasedReport");
        }
    }
}
