using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class TrainingTopicDto
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
        public bool isTrainingScheduled { get; set; }

    }
}
