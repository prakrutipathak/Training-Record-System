using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class AddProgramDetailsViewModel
    {
        [Required(ErrorMessage ="Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "Mode of preference is required.")]
        public string ModePreference { get; set; }

        public string TargetAudience { get; set; } = string.Empty;

        [Required]
        public int TrainerTopicId { get; set; }
    }
}
