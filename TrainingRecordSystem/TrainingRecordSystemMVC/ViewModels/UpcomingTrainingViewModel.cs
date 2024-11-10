namespace TrainingRecordSystemMVC.ViewModels
{
    public class UpcomingTrainingViewModel
    {
        public string TrainerName { get; set; }
        public string TopicName { get; set; }
        public string JobName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
