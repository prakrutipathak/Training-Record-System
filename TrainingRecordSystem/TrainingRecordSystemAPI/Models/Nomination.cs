using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class Nomination
    {
        [Key]
        public int NominationId { get; set; }

        public string ModePreference { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int TrainerId {  get; set; }
        public User User { get; set; }
       
        public int ParticipateId { get; set; }
        public Participate Participate { get; set; }

    }
}
