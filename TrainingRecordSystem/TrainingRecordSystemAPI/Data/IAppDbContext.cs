using Microsoft.EntityFrameworkCore;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data
{
    public interface IAppDbContext: IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TrainerTopic> TrainerTopics { get; set; }
        public DbSet<Participate> Participates { get; set; }
        public DbSet<TrainerProgramDetail> TrainerProgramDetails { get; set; }
        public DbSet<Nomination> Nominations { get; set; }
        IQueryable<ManagerReport> UpcomingTrainingProgram(int? jobId);

        string GetModeofTrainingByTopicId(int userId, int topicId);

        IQueryable<MonthlyAdminReportDto> MonthlyAdminReport(int userId, int? month, int? year);

        IQueryable<DaterangeBasedReportDto> DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate);


    }
}
