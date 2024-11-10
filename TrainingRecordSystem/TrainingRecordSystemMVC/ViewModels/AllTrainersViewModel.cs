using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class AllTrainersViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public int JobId { get; set; }

        public AllJobViewModel Job { get; set; }




    }
}
