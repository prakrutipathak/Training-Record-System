using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Implementation
{
    public class ManagerRepository: IManagerRepository
    {
        private readonly IAppDbContext _appDbContext;

        public ManagerRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Participate? GetParticipate(int id)
        {
            var contact = _appDbContext.Participates.FirstOrDefault(c => c.ParticipateId == id);
            return contact;
        }
        public IEnumerable<ManagerReport> UpcomingTrainingProgram(int? jobId)
        {
            var results = _appDbContext.UpcomingTrainingProgram(jobId);
            return results.ToList();
        }

        public string GetModeofTrainingByTopicId(int userId, int topicId)
        {
            var result = _appDbContext.GetModeofTrainingByTopicId(userId,topicId);
            return result;
        }

        public bool InsertParticipate(Participate participate)
        {
            var result = false;
            if (participate != null)
            {
                _appDbContext.Participates.Add(participate);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool AddNomination(Nomination nomination)
        {
            var result = false;
            if (nomination != null)
            {

                _appDbContext.Nominations.Add(nomination);

                _appDbContext.SaveChanges();
                result = true;
            }
            return result;

        }

        public bool ParticipateExists(string email)
        {
            var participate = _appDbContext.Participates.FirstOrDefault(c => c.Email == email);
            if (participate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
         public bool NominationExists(int topicId,int trainerid,int participateid)
        {
            var nomination = _appDbContext.Nominations.FirstOrDefault(c => c.TopicId==topicId && c.ParticipateId == participateid && c.TrainerId== trainerid);
            if (nomination != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int ManagerCountForParticipate(int managerId, int topicid,int trainerId)
        {
            return _appDbContext.Nominations.Include(c=>c.Participate).Where(b => b.Participate.UserId == managerId && b.TopicId == topicid && b.TrainerId==trainerId).Count();
        }
       
        public IEnumerable<Participate> GetParticipatesByManagerId(int managerId)
        {
            return _appDbContext.Participates.Include(c => c.Job).Where(c => c.UserId == managerId).ToList();

        }

    }
}
