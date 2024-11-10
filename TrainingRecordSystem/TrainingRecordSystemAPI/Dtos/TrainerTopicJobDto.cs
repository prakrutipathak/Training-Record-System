using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class TrainerTopicJobDto
    {
        public int TopicId { get; set; }

        public int JobId { get; set; }
        //Navigation
        public User User { get; set; }
        public Topic Topic { get; set; }

    }
}
