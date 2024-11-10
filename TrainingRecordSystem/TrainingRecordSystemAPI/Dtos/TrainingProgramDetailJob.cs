using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemAPI.Dtos
{
    public class TrainingProgramDetailJob
    {
        [Key]
        public int TrainerProgramDetailId { get; set; }

        public DateTime StartDate { get; set; }
       
        //foreign key
        public int TrainerTopicId { get; set; }
        public TrainingTopicDto TrainerTopic { get; set; }
    }
}
