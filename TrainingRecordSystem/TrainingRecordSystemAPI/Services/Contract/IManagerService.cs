using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface IManagerService
    {
        ServiceResponse<string> AddParticipate(Participate participate);
        ServiceResponse<string> NominateParticipant(NominateParticipateDto participate);
        ServiceResponse<IEnumerable<ManagerReport>> UpcomingTrainingProgram(int? jobId);
        ServiceResponse<IEnumerable<ParticipateDto>> GetParticipateByManageId(int managerId);
        ServiceResponse<ParticipateDto> GetParticipateById(int participantId);

        ServiceResponse<string> GetModeofTrainingByTopicId(int userId, int topicId);
    }
}
