using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Contract
{
    public interface ITopicRepository
    {
        IEnumerable<Topic> GetTopicsByJobId(int jobId);
        Topic GetTopicDetails(int topicId);
        IEnumerable<TrainerProgramDetail> GetTrainerTopicsByJobId(int jobId);
        IEnumerable<TrainerProgramDetail> GetTrainersByTopicId(int topicId);
    }
}
