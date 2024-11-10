using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface IAdminService
    {
        ServiceResponse<string> AddUser(AddUserDto userDataDto);

        ServiceResponse<IEnumerable<TrainerDto>> GetAllTrainer();
        ServiceResponse<IEnumerable<TrainerDto>> GetAllTrainerByPagination(int page, int pageSize);
        ServiceResponse<IEnumerable<MonthlyAdminReportDto>> MonthlyAdminReport(int userId, int? month, int? year);

        ServiceResponse<IEnumerable<DaterangeBasedReportDto>> DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate);


        ServiceResponse<int> TotalTrainer();


        ServiceResponse<string> AssignTopicToTrainer(AssignTrainingTopicDto assignDto);
        ServiceResponse<IEnumerable<AllJobsDto>> getAllJobs();
        ServiceResponse<UserDto> GetTrainerByLoginId(string id);
        ServiceResponse<string> UnassignTopic(int userId, int topicId);
    }
}
