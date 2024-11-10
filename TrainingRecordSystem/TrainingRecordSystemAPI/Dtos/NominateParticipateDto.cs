using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class NominateParticipateDto
    {

        public string ModePreference { get; set; }

        public int TopicId { get; set; }
        public int TrainerId { get; set; }
        public int ParticipateId { get; set; }
        public int UserId { get; set; }
      
    }
}
