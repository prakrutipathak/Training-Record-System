namespace TrainingRecordSystemMVC.ViewModels
{
    public class TrainingProgramDetailJViewModel
    {
      
        public int TrainerProgramDetailId { get; set; }


        public DateTime StartDate { get; set; }

        //foreign key
        public int TrainerTopicId { get; set; }
        public TrainingTopicViewModel TrainerTopic { get; set; }
    }
}
