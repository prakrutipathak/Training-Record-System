using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Dtos
{
    public class AssignTrainingTopicDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int TopicId { get; set; }
    }
}
