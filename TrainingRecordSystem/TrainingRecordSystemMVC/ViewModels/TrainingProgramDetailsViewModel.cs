namespace TrainingRecordSystemMVC.ViewModels
{
    public class TrainingProgramDetailsViewModel
    {
       public int TrainerProgramDetailId { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public DateTime StartTime { get; set; }
       public DateTime EndTime { get; set; }
       public int Duration { get; set; }
       public string ModePreference { get; set; }
       public string TargetAudience { get; set; }
       public int TrainerTopicId { get; set; }
        public TrainingTopicViewModel TrainerTopic { get; set; }
    } 
}

