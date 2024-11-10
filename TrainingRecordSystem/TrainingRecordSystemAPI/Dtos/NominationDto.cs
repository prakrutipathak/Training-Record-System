using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class NominationDto
    {
        public int NominationId { get; set; }

        public string ModePreference { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        public int ParticipateId { get; set; }
        public Participate Participate { get; set; }
    }
}
