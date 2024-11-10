using System.ComponentModel.DataAnnotations;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Dtos
{
    public class TrainerTopicDtoForProgramDetails
    {
        [Key]
        public int TrainerTopicId { get; set; }

        //Foreign Key
        public int UserId { get; set; }
        public int TopicId { get; set; }

        public int JobId { get; set; }
    }
}
