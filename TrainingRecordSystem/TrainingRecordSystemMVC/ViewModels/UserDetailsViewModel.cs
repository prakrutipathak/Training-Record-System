using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class UserDetailsViewModel
    {
        public int UserId { get; set; }

        [Required]
        public string LoginId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Role { get; set; }
        
        [Required]
        public int JobId { get; set; }
    }
}
