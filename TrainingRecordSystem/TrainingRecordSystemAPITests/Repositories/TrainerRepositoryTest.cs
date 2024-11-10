using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPITests.Repositories
{
    public class TrainerRepositoryTest
    {
        //----------GetAllTrainingTopicbyTrainerId -----//
        [Fact]
        public void GetAllTrainingTopicbyTrainerId_ReturnsCorrectTopics_WhenTopicsExists()
        {
           
            int page = 1;
            int pageSize = 6;

            var trainingTopics = new List<TrainerTopic>
            {
               new TrainerTopic{
                   TrainerTopicId=1,
                   UserId=5,
                   TopicId=2,
                   JobId = 1},
                new TrainerTopic{
                   TrainerTopicId=2,
                   UserId=6,
                   TopicId=3,
                   JobId = 2},

             }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerTopic>>();

            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(trainingTopics.Expression);
            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(trainingTopics.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerTopics).Returns(mockDbSet.Object);
            var target = new TrainerReository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllTrainingTopicbyTrainerId(5,page, pageSize);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Exactly(4));
        }

        // --------TotalCountofTrainingTopicbyTrainerId----//
        [Fact]
        public void TotalCountofTrainingTopicbyTrainerId_ReturnsCount_WhenTopicExist()
        {
            var trainingTopics = new List<TrainerTopic>
            {
               new TrainerTopic{
                   TrainerTopicId=1,
                   UserId=5,
                   TopicId=2,
                   JobId = 1},
                new TrainerTopic{
                   TrainerTopicId=2,
                   UserId=6,
                   TopicId=3,
                   JobId = 2},

             }.AsQueryable();


            var mockDbSet = new Mock<DbSet<TrainerTopic>>();
            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(trainingTopics.Provider);
            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(trainingTopics.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerTopics).Returns(mockDbSet.Object);
            var target = new TrainerReository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalCountofTrainingTopicbyTrainerId(5);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerTopics, Times.Once);

        }

         //---------------Get all Participants By Pagination-------------------
        [Fact]
        public void GetPaginatedParticipants_ReturnsCorrectParticipants_WithPagination()
        {
            // Arrange
            var participants = new List<Nomination>
            {
                new Nomination { NominationId=1, ModePreference = "Online" ,TopicId =1,Topic= new Topic(){TopicId=1,TopicName="Topic1",JobId=1,Job = new Job()},TrainerId=1,User = new User(),ParticipateId = 1,Participate = new Participate() },
                new Nomination { NominationId=2, ModePreference = "Offline" ,TopicId =1,Topic= new Topic(){TopicId=2,TopicName="Topic2",JobId=2,Job = new Job()},TrainerId=2,User = new User(),ParticipateId = 2,Participate = new Participate() },

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Expression).Returns(participants.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllParticipateByPagination(1, 2, "default");

            // Assert
            Assert.Equal(2, actual.Count());
            Assert.Equal("Topic2", actual.First().Topic.TopicName);
            Assert.Equal("Topic1", actual.Last().Topic.TopicName);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
     
        }



        [Fact]
        public void GetPaginatedParticipants_ReturnsCorrectParticipants_WithSortingAsc()
        {
            // Arrange
            var participants = new List<Nomination>
            {
                new Nomination { NominationId=1, ModePreference = "Online" ,TopicId =1,Topic= new Topic(){TopicId=1,TopicName="Topic1",JobId=1,Job = new Job()},TrainerId=1,User = new User(),ParticipateId = 1,Participate = new Participate() },
                new Nomination { NominationId=2, ModePreference = "Offline" ,TopicId =1,Topic= new Topic(){TopicId=2,TopicName="Topic2",JobId=2,Job = new Job()},TrainerId=2,User = new User(),ParticipateId = 2,Participate = new Participate() },

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Expression).Returns(participants.Expression);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.ElementType).Returns(participants.ElementType);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.GetEnumerator()).Returns(participants.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllParticipateByPagination(1, 4, "asc");

            // Assert
            Assert.Equal("Topic1", actual.First().Topic.TopicName);

            Assert.Equal("Topic2", actual.Last().Topic.TopicName);

            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }
        [Fact]
        public void GetPaginatedParticipants_ReturnsCorrectParticipants_WithSortingDesc()
        {
            // Arrange
            var participants = new List<Nomination>
            {
                new Nomination { NominationId=1, ModePreference = "Online" ,TopicId =1,Topic= new Topic(){TopicId=1,TopicName="Topic1",JobId=1,Job = new Job()},TrainerId=1,User = new User(),ParticipateId = 1,Participate = new Participate() },
                new Nomination { NominationId=2, ModePreference = "Offline" ,TopicId =1,Topic= new Topic(){TopicId=2,TopicName="Topic2",JobId=2,Job = new Job()},TrainerId=2,User = new User(),ParticipateId = 2,Participate = new Participate() },

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Expression).Returns(participants.Expression);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.ElementType).Returns(participants.ElementType);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.GetEnumerator()).Returns(participants.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act

            var actual = target.GetAllParticipateByPagination(1, 4, "desc");

            // Assert

            Assert.Equal("Topic2", actual.First().Topic.TopicName);
            Assert.Equal("Topic1", actual.Last().Topic.TopicName);


            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void GetPaginatedParticipants_ReturnsCorrectParticipants_WithSorting()
        {
            // Arrange
            var participants = new List<Nomination>
            {
                new Nomination { NominationId=1, ModePreference = "Online" ,TopicId =1,Topic= new Topic(){TopicId=1,TopicName="Topic1",JobId=1,Job = new Job()},TrainerId=1,User = new User(),ParticipateId = 1,Participate = new Participate() },
                new Nomination { NominationId=2, ModePreference = "Offline" ,TopicId =1,Topic= new Topic(){TopicId=2,TopicName="Topic2",JobId=2,Job = new Job()},TrainerId=2,User = new User(),ParticipateId = 2,Participate = new Participate() },

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Expression).Returns(participants.Expression);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.ElementType).Returns(participants.ElementType);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.GetEnumerator()).Returns(participants.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act

            var actual = target.GetAllParticipateByPagination(1, 4, "default");

            // Assert
            Assert.Equal(2, actual.Count());
            Assert.Equal(1, actual.ElementAt(1).ParticipateId);


            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        //-----------Total Participant count----------------

        [Fact]
        public void GetTotalParticipants_ReturnParicipants()
        {
            //Arrange
            var participants = new List<Nomination>
            {
                new Nomination { NominationId=1, ModePreference = "Online" ,TopicId =1,Topic= new Topic(){TopicId=1,TopicName="Topic1",JobId=1,Job = new Job()},TrainerId=1,User = new User(),ParticipateId = 1,Participate = new Participate() },
                new Nomination { NominationId=2, ModePreference = "Offline" ,TopicId =1,Topic= new Topic(){TopicId=2,TopicName="Topic2",JobId=2,Job = new Job()},TrainerId=2,User = new User(),ParticipateId = 2,Participate = new Participate() },

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Expression).Returns(participants.Expression);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.ElementType).Returns(participants.ElementType);
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.GetEnumerator()).Returns(participants.GetEnumerator());
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);
            //Act
            var actual = target.TotalNofParticipants();

            //Assert
            Assert.Equal(2, actual);

            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(1));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        // ----------------- AddTrainingProgramDetail -----------------
        [Fact]
        public void AddTrainingProgramDetail_ReturnsTrue_WhenDetailsAddedSuccessfully()
        {
            // Arrange
            TrainerProgramDetail trainerProgramDetail = new TrainerProgramDetail()
            {
                TrainerProgramDetailId = 1,
                TrainerTopicId = 1,
            };

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainerProgramDetail);

            // Assert
            Assert.True(actual);
            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
        }

        [Fact]
        public void AddTrainingProgramDetail_ReturnsFalse_WhenInsertionFails()
        {
            // Arrange
            TrainerProgramDetail trainerProgramDetail = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainerProgramDetail);

            // Assert
            Assert.False(actual);
        }

        // -------------- TrainingProgramDetailExists ------------
        [Fact]
        public void TrainingProgramDetailExists_ReturnsTrue_WhenDetailsExists()
        {
            // Arrange
            int topicId = 1;

            var detail = new TrainerProgramDetail()
            {
                TrainerTopicId = 1,
            };

            var detailList = new List<TrainerProgramDetail>()
            {
                detail,
                new TrainerProgramDetail()
                {
                    TrainerTopicId = 2,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(detailList.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(detailList.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.TrainingProgramDetailExists(topicId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }

        [Fact]
        public void TrainingProgramDetailExists_ReturnsFalse_WhenDetailsDontExists()
        {
            // Arrange
            int topicId = 3;

            var detail = new TrainerProgramDetail()
            {
                TrainerTopicId = 1,
            };

            var detailList = new List<TrainerProgramDetail>()
            {
                detail,
                new TrainerProgramDetail()
                {
                    TrainerTopicId = 2,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(detailList.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(detailList.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.TrainingProgramDetailExists(topicId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }


        // -------------- TrainingProgramDetailExists with userId ------------
        [Fact]
        public void TrainingProgramDetailExistswithUserId_ReturnsTrue_WhenDetailsExists()
        {
            // Arrange
            int topicId = 1;
            int userId = 2;

            var detail = new TrainerProgramDetail()
            {
                TrainerTopicId = 1,
                TrainerTopic = new TrainerTopic()
                {
                    UserId = 2
                }
            };

            var detailList = new List<TrainerProgramDetail>()
            {
                detail,
                new TrainerProgramDetail()
                {
                    TrainerTopicId = 2,
                    TrainerTopic = new TrainerTopic()
                    {
                        UserId = 3
                    }
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(detailList.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(detailList.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.TrainingProgramDetailExists(userId,topicId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }

        [Fact]
        public void TrainingProgramDetailExistswithUserId_ReturnsFalse_WhenDetailsDontExists()
        {
            // Arrange
            int userId = 2;
            int topicId = 3;

            var detail = new TrainerProgramDetail()
            {
                TrainerTopicId = 1,
            };

            var detailList = new List<TrainerProgramDetail>()
            {
                detail,
                new TrainerProgramDetail()
                {
                    TrainerTopicId = 2,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(detailList.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(detailList.Expression);
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.TrainingProgramDetailExists(userId,topicId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerProgramDetails, Times.Once);
        }
        // -------------- Get all Progrsm details ------------
        [Fact]
        public void GetAllTrainerProgramDetails_ReturnsData()
        {
            // Arrange

            var trainingProgram = new List<TrainerProgramDetail>
            {
                new TrainerProgramDetail
                {
                    TrainerTopic  = new TrainerTopic
                    {
                        TopicId = 2,
                        JobId = 1,
                        TrainerTopicId = 1,
                        UserId = 1,
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime  = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "online"

                },
                 new TrainerProgramDetail
                {
                    TrainerTopic  = new TrainerTopic
                    {
                        TopicId = 2,
                        JobId = 2,
                        TrainerTopicId = 2,
                        UserId = 2,
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime  = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "online"

                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(trainingProgram.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(m => m.Expression).Returns(trainingProgram.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllTrainerProgramDetails(1, 2);

            // Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);
     
        }
        [Fact]
        public void GetAllTrainerProgramDetails_ReturnsEmptyData()
        {
            // Arrange

            var trainingProgram = new List<TrainerProgramDetail>
            {
                new TrainerProgramDetail
                {
                    TrainerTopic  = new TrainerTopic
                    {
                        TopicId = 2,
                        JobId = 1,
                        TrainerTopicId = 1,
                        UserId = 1,
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime  = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "online"

                },
                 new TrainerProgramDetail
                {
                    TrainerTopic  = new TrainerTopic
                    {
                        TopicId = 2,
                        JobId = 2,
                        TrainerTopicId = 2,
                        UserId = 2,
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartTime  = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 1,
                    ModePreference = "online"

                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(m => m.Provider).Returns(trainingProgram.Provider);
            mockDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(m => m.Expression).Returns(trainingProgram.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllTrainerProgramDetails(1, 1);

            // Assert
            Assert.Null(actual);
            


            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Nomination>>().Verify(m => m.Expression, Times.Once);

        }
        // --------------Update Progrsm details ------------
     /*   [Fact]
        public void UpdateTrainingProgramDetail_ReturnsTrue_WhenDetailsUpdatedSuccessfully()
        {
            // Arrange
            TrainerProgramDetail trainerProgramDetail = new TrainerProgramDetail()
            {
                TrainerProgramDetailId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Duration = 1,
                ModePreference = "test",
                TargetAudience = "test",

                TrainerTopicId = 1,
                TrainerTopic = new TrainerTopic
                {
                    TopicId = 1,
                    UserId = 1,
                    JobId = 1,
                    TrainerTopicId = 1
                }
            };

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainerProgramDetail);

            // Assert
            Assert.True(actual);
            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
        }*/

        [Fact]
        public void UpdateTrainingProgramDetail_ReturnsFalse_WhenDetailsNotFound()
        {
            // Arrange

            var mockDbSet = new Mock<DbSet<TrainerProgramDetail>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.Setup(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new TrainerReository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(null);

            // Assert
            Assert.False(actual);
            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
        }
    }
}
