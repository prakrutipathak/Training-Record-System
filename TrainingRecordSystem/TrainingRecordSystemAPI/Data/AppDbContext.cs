using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TrainerTopic> TrainerTopics { get; set; }
        public DbSet<Participate> Participates { get; set; }
        public DbSet<TrainerProgramDetail> TrainerProgramDetails { get; set; }
        public DbSet<Nomination> Nominations { get; set; }


        public virtual IQueryable<ManagerReport> UpcomingTrainingProgram(int? jobId)
        {
            var jobIdParam = new SqlParameter("@JobId", jobId ?? (object)DBNull.Value);
            return Set<ManagerReport>().FromSqlRaw("dbo.UpcomingTrainingProgram  @JobId", jobIdParam);
        }

        public virtual string GetModeofTrainingByTopicId(int userId,int topicId)
        {
            var userIdParam = new SqlParameter("@UserId", userId);
            var topicIdParam = new SqlParameter("@TopicId", topicId);

            var result = Set<ModeOfPrefrenceDto>().FromSqlRaw("dbo.GetModeofTrainingByTopicId @TopicId,@UserId", topicIdParam, userIdParam).AsEnumerable().FirstOrDefault();

            return result?.ModePreference;
        }



        public virtual IQueryable<MonthlyAdminReportDto> MonthlyAdminReport(int userId,int? month,int? year)
        {
            var userIdParam = new SqlParameter("@UserId", userId);
            var monthParam = new SqlParameter("@Month", month ?? (object)DBNull.Value);
            var yearParam = new SqlParameter("@Year", year ?? (object)DBNull.Value);

            return Set<MonthlyAdminReportDto>().FromSqlRaw("dbo.MonthlyAdminReport  @UserId, @Month, @Year ", userIdParam, monthParam, yearParam);
        }

     
        public virtual IQueryable<DaterangeBasedReportDto> DaterangeBasedReport(int jobId,DateTime? startDate, DateTime? endDate)
        {
           

            var jobIdParam = new SqlParameter("@JobId", jobId);
            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);

            return Set<DaterangeBasedReportDto>().FromSqlRaw("dbo.DaterangeBasedReport  @JobId, @StartDate, @EndDate ", jobIdParam, startDateParam, endDateParam);
            }

            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nomination>()
               .HasOne(p => p.User)
               .WithMany(p => p.Nominations)
               .HasForeignKey(p => p.TrainerId)
                 .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<ModeOfPrefrenceDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<ManagerReport>().HasNoKey().ToView(null);
            modelBuilder.Entity<MonthlyAdminReportDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<DaterangeBasedReportDto>().HasNoKey().ToView(null);

        }

        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }
    }
}
