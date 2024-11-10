using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class UserDto
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(15)]
        public string LoginId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public int Role { get; set; }
        [Required]
        public bool Loginbit { get; set; }
        //foreign key
        public int JobId { get; set; }
    }
}
