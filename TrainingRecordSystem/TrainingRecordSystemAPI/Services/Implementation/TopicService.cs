using System.Diagnostics.Metrics;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public ServiceResponse<IEnumerable<TopicDto>> GetTopicsByJobId(int jobId)
        { 
            var response = new ServiceResponse<IEnumerable<TopicDto>>();
            try { 

            var topics = _topicRepository.GetTopicsByJobId(jobId);
            if(topics != null && topics.Any())
            {
                List<TopicDto> topicDtos = new List<TopicDto>();
                foreach (var topic in topics)
                { 
                    TopicDto topicDto = new TopicDto();
                    topicDto.TopicId = topic.TopicId;
                    topicDto.TopicName = topic.TopicName;
                    topicDto.JobId = topic.JobId;

                    topicDtos.Add(topicDto);
                }

                response.Success = true;
                response.Data = topicDtos;
                response.Message = "Topics found";
            }
            else
            {
                response.Success = false;
                response.Message = "No topics found";
            }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public ServiceResponse<IEnumerable<TrainingProgramDetailJob>> GetTrainerTopicsByJobId(int jobId)
        {
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>();
            try {
            var topics = _topicRepository.GetTrainerTopicsByJobId(jobId);
            if (topics != null && topics.Any())
            {
                List<TrainingProgramDetailJob> topicsDtos = new List<TrainingProgramDetailJob>();
                foreach (var topic in topics)
                {
                    if (topic.StartDate > DateTime.Now)
                    {
                        topicsDtos.Add(new TrainingProgramDetailJob()
                        {
                            TrainerTopicId = topic.TrainerTopicId,
                            StartDate=topic.StartDate,

                            TrainerTopic = new TrainingTopicDto
                            {
                               
                                Topic = new Topic
                                {

                                    TopicId = topic.TrainerTopic.Topic.TopicId,
                                    TopicName = topic.TrainerTopic.Topic.TopicName,
                                }
                            }


                        });
                    }
                }
                if (topicsDtos.Any())
                {
                    response.Data = topicsDtos;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found with start date after current date!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ServiceResponse<IEnumerable<TrainingProgramDetailJob>> GetTrainerByTopicId(int topicId)
        {
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>();
            try { 
            var topics = _topicRepository.GetTrainersByTopicId(topicId);
            if (topics != null && topics.Any())
            {
                List<TrainingProgramDetailJob> topicsDtos = new List<TrainingProgramDetailJob>();
                foreach (var topic in topics)
                {
                    if (topic.StartDate > DateTime.Now)
                    {
                        topicsDtos.Add(new TrainingProgramDetailJob()
                        {
                            TrainerTopicId = topic.TrainerTopicId,
                            StartDate = topic.StartDate,

                            TrainerTopic = new TrainingTopicDto
                            {
                                TopicId=topic.TrainerTopic.TopicId,
                                UserId=topic.TrainerTopic.UserId,
                                User = new User
                                {
                                    UserId = topic.TrainerTopic.User.UserId,
                                    FirstName = topic.TrainerTopic.User.FirstName,
                                    LastName = topic.TrainerTopic.User.LastName,
                                },

                               
                            }


                        });
                    }
                }
                if (topicsDtos.Any())
                {
                    response.Data = topicsDtos;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found with start date after current date!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
