using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class TrainerTopic
    {
        [Key]
        public int TrainerTopicId { get; set; }

        //Foreign Key
        public int UserId { get; set; }
        public int TopicId { get; set; }

        public int JobId { get; set; }
        //Navigation
        public User User { get; set; }
        public Topic Topic { get; set; }

        public Job Job { get; set; }
        public ICollection<TrainerProgramDetail> TrainerProgramDetail { get; set; }

    }
}
