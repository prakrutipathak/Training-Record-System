using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class AddParticipateViewModel
    {
        public int ParticipateId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must only contain alphabetic characters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must only contain alphabetic characters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(50)]
        public string Email { get; set; }
        public int UserId { get; set; }
        public UserViewModel? User { get; set; }

        [Required(ErrorMessage = "Job role is required")]
        public int JobId { get; set; }
        public JobViewModel? Job { get; set; }

    }
}
