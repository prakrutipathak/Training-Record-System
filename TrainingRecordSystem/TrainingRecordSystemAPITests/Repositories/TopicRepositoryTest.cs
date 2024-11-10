using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPITests.Repositories
{
    public class TopicRepositoryTest
    {
        // -------------- GetTopicsByJobId -----------------------
        [Fact]
        public void GetTopicsByJobId_ReturnsList_WhenTopicsExist()
        {
            // Arrange
            int jobId = 1;
            var topics = new List<Topic>()
            {
                new Topic()
                {
                    TopicId = 1,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 2,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 3,
                    JobId = 2,
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Topic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.Topics).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Topics, Times.Once);
        }

        [Fact]
        public void GetTopicsByJobId_ReturnsEmptyList_WhenNoTopicsExist()
        {
            // Arrange
            int jobId = 3;
            var topics = new List<Topic>()
            {
                new Topic()
                {
                    TopicId = 1,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 2,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 3,
                    JobId = 2,
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Topic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.Topics).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Topics, Times.Once);
        }

        // ------------- GetTopicDetails -----------------------
        [Fact]
        public void GetTopicDetails_ReturnsTopic_WhenTopicExists()
        {
            // Arrange
            int topicId = 1;
            var topic = new Topic()
            {
                TopicId = topicId,
                JobId = 1,
            };

            var topics = new List<Topic>()
            {
                topic,
                new Topic()
                {
                    TopicId = 2,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 3,
                    JobId = 2,
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Topic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.Topics).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTopicDetails(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(topic, actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Topics, Times.Once);
        }

        [Fact]
        public void GetTopicDetails_ReturnsNull_WhenTopicDoesntExists()
        {
            // Arrange
            int topicId = 1;
            var topic = new Topic()
            {
                TopicId = 4,
                JobId = 1,
            };

            var topics = new List<Topic>()
            {
                topic,
                new Topic()
                {
                    TopicId = 2,
                    JobId = 1,
                },
                new Topic()
                {
                    TopicId = 3,
                    JobId = 2,
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Topic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.Topics).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTopicDetails(topicId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Topics, Times.Once);
        }
        //------------------------------GetTrainerTopicsByJobId-------------------
        [Fact]
        public void GetTrainerTopicsByJobId_ReturnsList_WhenTopicsExist()
        {
            // Arrange
            int jobId = 1;
            var topics = new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail
                {
                    TrainerProgramDetailId = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "offline",
                    TargetAudience = "ello",
                    TrainerTopic = new TrainerTopic
                    {
                        TopicId = 1,
                        JobId = 1,
                        Topic = new Topic
                        {
                            TopicId = 1,
                            TopicName = "test",
                            JobId = 1
                        }
                    },


                },
                   new TrainerProgramDetail
                {
                    TrainerProgramDetailId = 2,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "offline",
                    TargetAudience = "ello",
                    TrainerTopic = new TrainerTopic
                    {
                        TopicId = 1,
                        JobId = 1,
                        Topic = new Topic
                        {
                            TopicId = 2,
                            TopicName = "test",
                            JobId = 1
                        }
                    },


                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }
        [Fact]
        public void GetTrainerTopicsByJobId_ReturnsList_WhenNoTopicsExist()
        {
            // Arrange
            int jobId = 3;
            var topics = new List<TrainerProgramDetail>()
            {
                new TrainerProgramDetail
                {
                    TrainerProgramDetailId = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "offline",
                    TargetAudience = "ello",
                    TrainerTopic = new TrainerTopic
                    {
                        TopicId = 1,
                        JobId = 1,
                        Topic = new Topic
                        {
                            TopicId = 1,
                            TopicName = "test",
                            JobId = 1
                        }
                    },
                    
                    
                },
                   new TrainerProgramDetail
                {
                    TrainerProgramDetailId = 2,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "offline",
                    TargetAudience = "ello",
                    TrainerTopic = new TrainerTopic
                    {
                        TopicId = 1,
                        JobId = 1,
                        Topic = new Topic
                        {
                            TopicId = 2,
                            TopicName = "test",
                            JobId = 1
                        }
                    },


                }
            }.AsQueryable();

            /*var fixture = new Fixture();
            var topic1 = fixture.Create<IEnumerable<TrainerTopic>>().AsQueryable();*/

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId);

            // Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }
        //------------------------------GetTrainersByTopicId----------------------
        [Fact]
        public void GetTrainersByTopicId_ReturnsList_WhenTopicsExist()
        {
            // Arrange
            int topicId = 1;
            var trainerTopics = new List<TrainerProgramDetail>()
    {
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 1 }
        },
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 1 }
        },
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 3 }
        }
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            mockDbSet.As<IQueryable>().Setup(m => m.Provider).Returns(trainerTopics.Provider);
            mockDbSet.As<IQueryable>().Setup(m => m.Expression).Returns(trainerTopics.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object); 

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainersByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count()); 
            mockDbSet.As<IQueryable>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(m => m.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }
        [Fact]
        public void GetTrainersByTopicId_ReturnsList_WhenTopicsNoExist()
        {
            // Arrange
            int topicId = 4;
            var trainerTopics = new List<TrainerProgramDetail>()
    {
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 1 }
        },
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 2 }
        },
        new TrainerProgramDetail()
        {
            TrainerTopic = new TrainerTopic() { TopicId = 3 }
        }
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            mockDbSet.As<IQueryable>().Setup(m => m.Provider).Returns(trainerTopics.Provider);
            mockDbSet.As<IQueryable>().Setup(m => m.Expression).Returns(trainerTopics.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object); // Assuming TrainerProgramDetails is DbSet in your context

            var target = new TopicRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainersByTopicId(topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count()); 
            mockDbSet.As<IQueryable>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable>().Verify(m => m.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }


    }
}
