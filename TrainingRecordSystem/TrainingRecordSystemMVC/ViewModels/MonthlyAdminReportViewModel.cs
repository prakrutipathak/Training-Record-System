namespace TrainingRecordSystemMVC.ViewModels
{
    public class MonthlyAdminReportViewModel
    {
        public string TopicName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Duration { get; set; }
        public string ModePreference { get; set; }

        public int TotalParticipateNo { get; set; }


    }
}
