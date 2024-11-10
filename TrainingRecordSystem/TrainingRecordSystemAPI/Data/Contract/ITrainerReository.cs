using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Contract
{
    public interface ITrainerReository
    {
        IEnumerable<TrainerTopic> GetAllTrainingTopicbyTrainerId(int trainerId, int page, int pageSize);

        int TotalCountofTrainingTopicbyTrainerId(int trainerId);

        IEnumerable<Nomination> GetAllParticipateByPagination(int page, int pageSize, string sort_name);

        int TotalNofParticipants();
        bool AddTrainingProgramDetail(TrainerProgramDetail trainingProgramDetail);
        bool TrainingProgramDetailExists(int TrainerTopicId);
        TrainerProgramDetail GetAllTrainerProgramDetails(int userId, int topicId);
        bool TrainingProgramDetailExists(int userId, int TrainerTopicId);
        bool UpdateTrainingProgramDetails(TrainerProgramDetail trainingProgramDetail);
    }
}
