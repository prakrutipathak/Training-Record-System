using Microsoft.EntityFrameworkCore;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Implementation
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IAppDbContext _context;

        public TopicRepository(IAppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        public IEnumerable<Topic> GetTopicsByJobId(int jobId)
        {
            var topics = _context.Topics.Where(c => c.JobId == jobId);
            return topics.ToList();
        }
        public IEnumerable<TrainerProgramDetail> GetTrainerTopicsByJobId(int jobId)
        {
            var topics = _context.TrainerProgramDetails.Where(c => c.TrainerTopic.JobId == jobId).Include(c=>c.TrainerTopic.Topic).Include(c=>c.TrainerTopic);
            return topics.ToList();
        }
        public IEnumerable<TrainerProgramDetail> GetTrainersByTopicId(int topicId)
        {
            var topics = _context.TrainerProgramDetails.Where(c => c.TrainerTopic.TopicId == topicId).Include(c => c.TrainerTopic.User).Include(c => c.TrainerTopic);
            return topics.ToList();
        }

        public Topic GetTopicDetails(int topicId)
        {
            Topic? topic = _context.Topics.FirstOrDefault(c => c.TopicId == topicId);
            return topic;
        }
    }
}
