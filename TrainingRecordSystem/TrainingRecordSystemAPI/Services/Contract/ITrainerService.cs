using TrainingRecordSystemAPI.Dtos;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface ITrainerService
    {
        ServiceResponse<IEnumerable<TrainingTopicDto>> GetAllTrainingTopicbyTrainerId(int trainerId, int page, int pageSize);

        ServiceResponse<int> TotalCountofTrainingTopicbyTrainerId(int trainerId);

        ServiceResponse<IEnumerable<NominationDto>> GetAllParticipantsByPAgination(int page, int pageSize, string sort_name);
        ServiceResponse<int> TotalParticipants();
        ServiceResponse<string> AddTrainingProgramDetail(AddTrainingProgramDetailDto trainingProgramDetailDto);
        ServiceResponse<TrainingProgramDetailsDto> GetAllTraniningProgramDetails(int userId, int topicId);
        ServiceResponse<string> UpdateTrainingProgramDetails(UpdateTrainingProgramDetailDto trainingProgramDetailDto);
    }
}
