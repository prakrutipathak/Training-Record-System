using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Contract
{
    public interface IManagerRepository
    {
        Participate? GetParticipate(int id);
         bool InsertParticipate(Participate participate);
        bool AddNomination(Nomination nomination);

         bool ParticipateExists(string email);
        bool NominationExists(int topicId, int trainerid, int participateid);
        int ManagerCountForParticipate(int managerId, int topicid, int trainerId);
        IEnumerable<ManagerReport> UpcomingTrainingProgram(int? jobId);
        IEnumerable<Participate> GetParticipatesByManagerId(int managerId);

        string GetModeofTrainingByTopicId(int userId, int topicId);
    }
}
