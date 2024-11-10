namespace TrainingRecordSystemMVC.ViewModels
{
    public class DaterangeBasedReportViewModel
    {
        public string TopicName { get; set; }

        public string TrainerName { get; set; }
        public int TotalParticipateNo { get; set; }



        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Duration { get; set; }

        public string ModePreference { get; set; }
    }
}
