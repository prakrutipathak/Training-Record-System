namespace TrainingRecordSystemMVC.ViewModels
{
    public class TopicViewModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        //foreign key
        public int JobId { get; set; }
        //navigation
        public JobViewModel Job { get; set; }
    }
}
