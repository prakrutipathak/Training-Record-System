using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        //foreign key
        public int JobId { get; set; }
        //navigation
        public Job Job { get; set; }
         public ICollection<TrainerTopic> TrainerTopics { get; set; }
        public ICollection<Nomination> Nominations { get; set; }




    }
}
