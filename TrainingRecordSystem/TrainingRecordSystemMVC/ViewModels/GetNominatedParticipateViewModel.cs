namespace TrainingRecordSystemMVC.ViewModels
{
    public class GetNominatedParticipateViewModel
    {
        public int NomiationId { get; set; }

        public string ModePreference { get; set; }

        public int TopicId { get; set; }
        public TopicViewModel Topic { get; set; }

        public int ParticipateId { get; set; }
        public ParticipateViewModel Participate { get; set; }
    }
}
