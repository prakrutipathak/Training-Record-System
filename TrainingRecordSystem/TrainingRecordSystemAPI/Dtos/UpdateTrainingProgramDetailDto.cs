using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Dtos
{
    public class UpdateTrainingProgramDetailDto
    {
        [Required]
        public int TrainerProgramDetailId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public string ModePreference { get; set; }

        public string TargetAudience { get; set; }

        [Required]
        public int TrainerTopicId { get; set; }
    }
}
