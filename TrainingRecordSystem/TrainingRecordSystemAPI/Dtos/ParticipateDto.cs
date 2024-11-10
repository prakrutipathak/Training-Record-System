using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class ParticipateDto
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
        public User User { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
  
    }
}
