using System.ComponentModel.DataAnnotations;

namespace TrainingRecordSystemMVC.ViewModels
{
    public class TopicByPaginationViewModel
    {
        public int TrainerTopicId { get; set; }

        //Foreign Key
        public int UserId { get; set; }
        public int TopicId { get; set; }

        public int JobId { get; set; }
        //Navigation
        public UserViewModel User { get; set; }
        public TopicViewModel Topic { get; set; }

        public JobViewModel Job { get; set; }
        public bool isTrainingScheduled { get; set; }
    }
}
