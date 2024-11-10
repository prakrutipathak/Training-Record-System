namespace TrainingRecordSystemAPI.Dtos
{
    public class MonthlyAdminReportDto
    {
        public string TopicName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Duration { get; set; }
        public string ModePreference { get; set; }

        public int TotalParticipateNo { get; set; }



    }
}
