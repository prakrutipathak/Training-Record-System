using Microsoft.EntityFrameworkCore;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Contract
{
    public interface IAdminRepository
    {
        bool InsertUser(User user);
        bool UserExists(string email);

        bool UserExists(int userId, string email);

        IEnumerable<User> GetAllTrainer();
        IEnumerable<User> GetAllTrainerPagination(int page, int pageSize);

        IEnumerable<MonthlyAdminReportDto> MonthlyAdminReport(int userId, int? month, int? year);

        IEnumerable<DaterangeBasedReportDto> DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate);
        int TotalNoOfTrainer();
        bool AssignTopicToTrainer(TrainerTopic trainerTopic);
        User GetUserDetails(int userId);
        bool TopicAlreadyAssigned(int userId, int topicId);
        IEnumerable<Job> GetAllJobs();

        User GetTrainerDetailsByLoginId(string LoginId);
        bool UnassignTopic(int userId, int topicId);
    }
}
