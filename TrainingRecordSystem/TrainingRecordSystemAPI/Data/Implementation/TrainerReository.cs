using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Implementation
{
    public class TrainerReository : ITrainerReository
    {
        private readonly IAppDbContext _appDbContext;

        public TrainerReository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public IEnumerable<TrainerTopic> GetAllTrainingTopicbyTrainerId(int trainerId, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;


            IEnumerable<TrainerTopic> trainerTopic = _appDbContext.TrainerTopics.Include(t => t.User).Include(t => t.Job).Include(t => t.Topic).Where(c => c.UserId == trainerId);

            return trainerTopic.Skip(skip).Take(pageSize).ToList();
        }


        public int TotalCountofTrainingTopicbyTrainerId(int trainerId)
        {
            return _appDbContext.TrainerTopics.Where(c => c.UserId == trainerId).Count();

        }

        //----------------GetAll Participates By Pagination-------------
        public IEnumerable<Nomination> GetAllParticipateByPagination(int page, int pageSize, string sort_name)
        {
            //var participants = _appDbContext.Nominations.Include(p => p.User).Include(p => p.Job).Include(p => p.Nominations).
            var participants = _appDbContext.Nominations.Include(p => p.Topic).Include(p => p.Participate).Include(p => p.Participate.User).Include(p => p.Participate.Job).
                AsQueryable();

            if (sort_name == "asc")
            {
                participants = participants.OrderBy(p => p.Topic.TopicName);
            }
            else if (sort_name == "desc")
            {
                participants = participants.OrderByDescending(p => p.Topic.TopicName);
            }
            else
            {
                participants = participants.OrderByDescending(p => p.NominationId);
            }

            int skip = (page - 1) * pageSize;

            return participants
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public int TotalNofParticipants()
        {
            return _appDbContext.Nominations.Count();
        }




        public bool AddTrainingProgramDetail(TrainerProgramDetail trainingProgramDetail)
        {
            var result = false;
            if (trainingProgramDetail != null)
            {
                _appDbContext.TrainerProgramDetails.Add(trainingProgramDetail);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool TrainingProgramDetailExists(int TrainerTopicId)
        {
            var trainingProgramDetails = _appDbContext.TrainerProgramDetails.FirstOrDefault(c => c.TrainerTopicId == TrainerTopicId);
            if (trainingProgramDetails != null)
            {
                return true;
            }

            return false;
        }

        public bool TrainingProgramDetailExists(int userId,int TrainerTopicId)
        {
            var trainingProgramDetails = _appDbContext.TrainerProgramDetails.FirstOrDefault(c => c.TrainerTopicId == TrainerTopicId && c.TrainerTopic.UserId == userId);
            if (trainingProgramDetails != null)
            {
                return true;
            }

            return false;
        }

        public TrainerProgramDetail GetAllTrainerProgramDetails(int userId,int topicId)
        {
            var response = _appDbContext.TrainerProgramDetails.Include(c => c.TrainerTopic).FirstOrDefault(c => c.TrainerTopic.TopicId == topicId && c.TrainerTopic.UserId == userId);

            return response;
        }

        [ExcludeFromCodeCoverage]
        public bool UpdateTrainingProgramDetails(TrainerProgramDetail trainingProgramDetail)
        {
            var result = false;
            if (trainingProgramDetail != null)
            {
                var existingDetails = _appDbContext.TrainerProgramDetails.Local.SingleOrDefault(e => e.TrainerProgramDetailId == trainingProgramDetail.TrainerProgramDetailId);
                if(existingDetails != null)
                {
                    _appDbContext.Entry(existingDetails).State = EntityState.Detached;
                }

                //_appDbContext.TrainerProgramDetails.Update(trainingProgramDetail);
                _appDbContext.Attach(trainingProgramDetail);
                _appDbContext.Entry(trainingProgramDetail).State = EntityState.Modified;
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}
