using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class AllJobViewModel
    {
        [Required]
        public int JobId { get; set; }
        [Required(ErrorMessage = "Job name is required")]
        public string JobName { get; set; }
    }
}
