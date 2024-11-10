using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class ParticipateViewModel
    {
        public int ParticipantId { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        //foreign key and navigation
        public int UserId { get; set; }
        public UserViewModel User { get; set; }
        public int JobId { get; set; }
        public JobViewModel Job { get; set; }
    }
}
