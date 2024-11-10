using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPITests.Services
{
    public class TopicServiceTest:IDisposable
    {
        private readonly Mock<ITopicRepository> mockTopicRepository;
        public TopicServiceTest() 
        {
            mockTopicRepository= new Mock<ITopicRepository>();
        }
        // ------------ GetTopicsByJobId ----------------
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTopicsByJobId_ReturnsTopics_WhenTopicsFound()
        {
            // Arrange
            int jobId = 1;

            var topics = new List<Topic>()
            {
                new Topic()
                {
                    TopicId = 1,
                    TopicName = "Topic 1",
                    JobId = jobId,
                },
                new Topic()
                {
                    TopicId = 2,
                    TopicName = "Topic 2",
                    JobId = jobId,
                },
            };

            var topicDtos = new List<TopicDto>()
            {
                new TopicDto()
                {
                    TopicId = 1,
                    TopicName = "Topic 1",
                    JobId = jobId,
                },
                new TopicDto()
                {
                    TopicId = 2,
                    TopicName = "Topic 2",
                    JobId = jobId,
                },
            };

            var mockResponse = new ServiceResponse<IEnumerable<TopicDto>>()
            {
                Success = true,
                Data = topicDtos,
                Message = "Topics found",
            };
            mockTopicRepository.Setup(c => c.GetTopicsByJobId(jobId)).Returns(topics);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }

        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTopicsByJobId_ReturnsError_WhenNoTopicsFound()
        {
            // Arrange
            int jobId = 1;

            IEnumerable<Topic> topics = new List<Topic>();

            var mockResponse = new ServiceResponse<IEnumerable<TopicDto>>()
            {
                Success = false,
                Message = "No topics found",
            };

            mockTopicRepository.Setup(c => c.GetTopicsByJobId(jobId)).Returns(topics);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }

        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTopicsByJobId_ReturnsError_WhenTopicsAreNull()
        {
            // Arrange
            int jobId = 1;

            IEnumerable<Topic> topics = null;

            var mockResponse = new ServiceResponse<IEnumerable<TopicDto>>()
            {
                Success = false,
                Message = "No topics found",
            };
            mockTopicRepository.Setup(c => c.GetTopicsByJobId(jobId)).Returns(topics);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTopicsByJobId_WhenThrowException()
        {
            // Arrange
            int jobId = 1;

            mockTopicRepository.Setup(c => c.GetTopicsByJobId(jobId)).Throws(new Exception());

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTopicRepository.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }
        // ------------ GetTrainerTopicsByJobId ----------------
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerTopicsByJobId_ReturnsTopics_WhenTopicsFound()
        {
            // Arrange
            int jobId = 1;
           
            var trainingDetail= new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail()
                {
                   StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                    TrainerTopic= new TrainerTopic
                    {
                    
                        Topic =new Topic()
                            {
                                TopicId = 1,
                                TopicName = "Topic 1",
                                JobId = jobId,
                            }
                    }

                },
                 new TrainerProgramDetail()
                {    StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                     TrainerTopic= new TrainerTopic
                    {
                        Topic =new Topic()
                            {
                                TopicId = 1,
                                TopicName = "Topic 1",
                                JobId = jobId,
                            }
                    }
                },
            };

            var mockResponse = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = true,

            };
            mockTopicRepository.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Returns(trainingDetail);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerTopicsByJobId_ReturnsNoTopics_WhenStartDateIsLessThanCurrentDate()
        {
            // Arrange
            int jobId = 1;

            var trainingDetail = new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail()
                {
                   StartDate = DateTime.Parse("2024-07-16", System.Globalization.CultureInfo.CurrentCulture),
                    TrainerTopic= new TrainerTopic
                    {

                        Topic =new Topic()
                            {
                                TopicId = 1,
                                TopicName = "Topic 1",
                                JobId = jobId,
                            }
                    }

                },
                 new TrainerProgramDetail()
                {    StartDate = DateTime.Parse("2024-07-16", System.Globalization.CultureInfo.CurrentCulture),
                     TrainerTopic= new TrainerTopic
                    {
                        Topic =new Topic()
                            {
                                TopicId = 1,
                                TopicName = "Topic 1",
                                JobId = jobId,
                            }
                    }
                },
            };

            var mockResponse = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = false,
                Message= "No record found with start date after current date!",

            };

            mockTopicRepository.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Returns(trainingDetail);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerTopicsByJobId_ReturnsTopics_WhenNoTopicsFound()
        {
            // Arrange
            int jobId = 1;


            var topics = new List<TrainerProgramDetail>()
            {

            };


            var mockResponse = new ServiceResponse<IEnumerable<TrainerTopicJobDto>>()
            {
                Success = false,
                Message = "No record found!"

            };

           
            mockTopicRepository.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Returns(topics);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerTopicsByJobId_WhenThrowException()
        {
            // Arrange
            int jobId = 1;

            mockTopicRepository.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Throws(new Exception());

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTopicRepository.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }
        //------------------------GetTrainerByTopicId------------------------------
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerByTopicId_ReturnsTopics_WhenTopicsFound()
        {
            // Arrange
            int topicId = 1;

            var trainingDetail = new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail()
                {
                    TrainerTopicId=1,
                   StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                    TrainerTopic= new TrainerTopic
                    {
                        TopicId = 1,
                        UserId=4,

                        User =new User()
                            {
                               UserId = 4,
                                FirstName = "FirstName",
                                LastName = "LastName",
                            }
                    }

                },
                 new TrainerProgramDetail()
                {     TrainerTopicId=2,
                     StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                      TrainerTopic= new TrainerTopic
                    {
                        TopicId = 2,
                        UserId=4,

                        User =new User()
                            {
                               UserId = 4,
                                FirstName = "FirstName",
                                LastName = "LastName",
                            }
                    }
                },
            };

            var mockResponse = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = true,

            };
            mockTopicRepository.Setup(c => c.GetTrainersByTopicId(topicId)).Returns(trainingDetail);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainersByTopicId(topicId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerByTopicId_ReturnsNoTopics_WhenStartDateIsLessThanCurrentDate()
        {
            // Arrange
            int topicId = 1;

            var trainingDetail = new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail()
                {
                    TrainerTopicId=1,
                   StartDate = DateTime.Parse("2024-07-16", System.Globalization.CultureInfo.CurrentCulture),
                     TrainerTopic= new TrainerTopic
                    {
                        TopicId = 1,
                        UserId=4,

                        User =new User()
                            {
                               UserId = 4,
                                FirstName = "FirstName",
                                LastName = "LastName",
                            }
                    }

                },
                 new TrainerProgramDetail()

                {
                     TrainerTopicId=2,
                     StartDate = DateTime.Parse("2024-07-16", System.Globalization.CultureInfo.CurrentCulture),
                      TrainerTopic= new TrainerTopic
                    {
                        TopicId = 2,
                        UserId=4,

                        User =new User()
                            {
                               UserId = 4,
                                FirstName = "FirstName",
                                LastName = "LastName",
                            }
                    }
                },
            };

            var mockResponse = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = false,
                Message = "No record found with start date after current date!",

            };
            mockTopicRepository.Setup(c => c.GetTrainersByTopicId(topicId)).Returns(trainingDetail);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainersByTopicId(topicId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerByTopicId_ReturnsTopics_WhenNoTopicsFound()
        {
            // Arrange
            int topicId = 1;


            var topics = new List<TrainerProgramDetail>()
            {

            };


            var mockResponse = new ServiceResponse<IEnumerable<TrainerTopicJobDto>>()
            {
                Success = false,
                Message = "No record found!"

            };
            mockTopicRepository.Setup(c => c.GetTrainersByTopicId(topicId)).Returns(topics);

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTopicRepository.Verify(c => c.GetTrainersByTopicId(topicId), Times.Once);
        }
        [Fact]
        [Trait("Topic", "TopicServiceTests")]
        public void GetTrainerByTopicId_WhenThrowException()
        {
            // Arrange
            int topicId = 1;
            mockTopicRepository.Setup(c => c.GetTrainersByTopicId(topicId)).Throws(new Exception());

            var target = new TopicService(mockTopicRepository.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTopicRepository.Verify(c => c.GetTrainersByTopicId(topicId), Times.Once);
        }
        public void Dispose()
        {
            mockTopicRepository.VerifyAll();
        }
    }
}
