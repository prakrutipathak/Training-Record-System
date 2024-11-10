using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class User
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
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public int Role {  get; set; }
        [Required]
        public bool Loginbit { get; set; } = false;
        //foreign key
        public int JobId { get; set; }
        //navigation
        public Job Job { get; set; }
        public ICollection<TrainerTopic> TrainerTopics { get; set; }
        public ICollection<Nomination> Nominations { get; set; }
        public ICollection<Participate> Participates { get; set; }
      
    }
}
