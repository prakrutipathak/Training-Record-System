using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class TrainingProgramDetailsDto
    {
        [Key]
        public int TrainerProgramDetailId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public string ModePreference { get; set; }
        public string TargetAudience { get; set; }
        //foreign key
        public int TrainerTopicId { get; set; }
        public TrainerTopicDtoForProgramDetails TrainerTopic { get; set; }
    }
}
