namespace TrainingRecordSystemAPI.Models
{
    public class ManagerReport
    {
        public string TrainerName {  get; set; }
        public string TopicName { get; set;}
        public string JobName { get; set;}

        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
    }
}
