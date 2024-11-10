using TrainingRecordSystemAPI.Dtos;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface ITopicService
    {
        ServiceResponse<IEnumerable<TopicDto>> GetTopicsByJobId(int jobId);
        ServiceResponse<IEnumerable<TrainingProgramDetailJob>> GetTrainerTopicsByJobId(int jobId);
        ServiceResponse<IEnumerable<TrainingProgramDetailJob>> GetTrainerByTopicId(int topicId);
    }
}
