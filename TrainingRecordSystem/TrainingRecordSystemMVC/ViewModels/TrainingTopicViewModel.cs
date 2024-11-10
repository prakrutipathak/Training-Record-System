namespace TrainingRecordSystemMVC.ViewModels
{
    public class TrainingTopicViewModel
    {
        public int TrainerTopicId { get; set; }

        //Foreign Key
        public int UserId { get; set; }
        public int TopicId { get; set; }

        public int JobId { get; set; }
        //Navigation
        public TopicViewModel Topic { get; set; }

        public JobViewModel Job { get; set; }
        public bool isTrainingScheduled { get; set; }
    }
}
