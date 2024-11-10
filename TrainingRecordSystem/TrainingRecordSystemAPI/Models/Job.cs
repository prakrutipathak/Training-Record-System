using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string JobName { get; set; }
        public ICollection<TrainerTopic> TrainerTopics { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Participate> Participates { get; set; }
    }
}
