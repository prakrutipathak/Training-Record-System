using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Models
{
    public class Participate
    {

        [Key]
        public int ParticipateId { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        //foreign key and navigation
        public int UserId { get; set; }
        public User User { get; set; }
        public int JobId {  get; set; }
        public Job Job { get; set; }

        public ICollection<Nomination> Nominations { get; set; }



    }
}
