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
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPITests.Repositories
{
    public class ManagerRepositoryTest
    {
        [Fact]
        public void GetParticipate_WhenParticpateIsNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Participate>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Participate>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Participates).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);
            //Act
            var actual = target.GetParticipate(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Participates, Times.Once);

        }
        [Fact]
        public void GetParticipate_WhenParticipateIsNotNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Participate>()
            {
              new Participate { ParticipateId = 1, FirstName = "Participate 1" },
                new Participate { ParticipateId = 2, FirstName = "Participate 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Participate>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Participates).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);
            //Act
            var actual = target.GetParticipate(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Participates, Times.Once);

        }
        [Fact]
        public void InsertParticipate_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Participate>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Participates).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ManagerRepository(mockAppDbContext.Object);
            var participate = new Participate
            {
                ParticipateId = 1,
                FirstName = "P1"
            };


            //Act
            var actual = target.InsertParticipate(participate);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(participate), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertParticipate_ReturnsFalse()
        {
            //Arrange
            Participate participate = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.InsertParticipate(participate);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddNomination_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Nomination>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Nominations).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ManagerRepository(mockAppDbContext.Object);
            var participate = new Nomination
            {
                ParticipateId = 1,
                
            };


            //Act
            var actual = target.AddNomination(participate);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(participate), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void AddNomination_ReturnsFalse()
        {
            //Arrange
            Nomination participate = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.AddNomination(participate);
            //Assert
            Assert.False(actual);
        }
        [Fact]
        public void ParticipateExists_ReturnsTrue()
        {
            //Arrange
            var email = "abc@gmail.com";
            var participates = new List<Participate>
            {
                new Participate { ParticipateId = 1, FirstName = "Participate 1", Email="abc@gmail.com"},
                new Participate { ParticipateId = 2, FirstName = "Participate 2", Email="xyz@gmail.com" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Participate>>();
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(participates.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Participates).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.ParticipateExists(email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Participates, Times.Once);
        }

        [Fact]
        public void ParticipateExists_ReturnsFalse()
        {
            //Arrange
            var email = "abc@gmail.com";
            var participates = new List<Participate>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Participate>>();
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(participates.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Participates).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.ParticipateExists(email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Participates, Times.Once);
        }

        [Fact]
        public void NominationExists_ReturnsFalse()
        {
            //Arrange

            var topicId = 1;
            var participateid = 1;
            var trainerid = 1;
            var participates = new List<Nomination>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Expression).Returns(participates.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.NominationExists(topicId,participateid,trainerid);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Nominations, Times.Once);
        }

        [Fact]
        public void NominationExists_ReturnsTrue()
        {
            //Arrange

            var topicId = 1;
            var participateid = 1;
            var trainerid = 1;
            var participates = new List<Nomination>
            {
                new Nomination { ParticipateId = 1,TopicId=1,TrainerId=1},
                new Nomination { ParticipateId = 2,TopicId=2,TrainerId=1},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Nomination>>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Expression).Returns(participates.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.NominationExists(topicId, participateid, trainerid);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Nominations, Times.Once);
        }
        [Fact]
        public void UpcomingTrainingProgram_WhenExists()
        {
            int jobId = 1;
            //Arrange
            var mockAbContext = new Mock<IAppDbContext>();
            var expectedResults = new List<ManagerReport>
             {
               new ManagerReport { TrainerName="T1",JobName="J1",TopicName="Topic 1" },
               new ManagerReport { TrainerName="T2",JobName="J2",TopicName="Topic 2" }
             }.AsQueryable();
            mockAbContext.Setup(c => c.UpcomingTrainingProgram(jobId)).Returns(expectedResults);
            var target = new ManagerRepository(mockAbContext.Object);
            //Act
            var actual = target.UpcomingTrainingProgram(jobId);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResults.Count(), actual.Count());
            mockAbContext.Verify(c => c.UpcomingTrainingProgram(jobId),Times.Once);

        }

        [Fact]
        public void UpcomingTrainingProgram_WhenNotExists()
        {
            //Arrange
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ManagerRepository(mockAbContext.Object);
            //Act
            var actual = target.UpcomingTrainingProgram(null);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());


        }
        [Fact]
        public void ManagerCountForParticipate_WithTopicId()
        {
            // Arrange
            int managerId = 1;
            int topicId = 2;
            int trainerId = 2;
            var participates = new List<Nomination>
    {
        new Nomination { Participate=new Participate{ UserId = managerId }, TopicId = topicId,TrainerId=2 },
        new Nomination { Participate=new Participate{ UserId = managerId }, TopicId = topicId ,TrainerId=2},
        new Nomination { Participate=new Participate{ UserId = managerId }, TopicId = 3 ,TrainerId=2} 
    }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Nomination>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Expression).Returns(participates.Expression);

            mockAbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            // Act
            int count = target.ManagerCountForParticipate(managerId, topicId,trainerId);

            // Assert
            Assert.Equal(2, count);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Nominations, Times.Once);
        }
        [Fact]
        public void ManagerCountForParticipate_NoParticipations()
        {
            // Arrange
            int managerId = 1;
            int topicId = 2;
            int trainerId = 2;
            var mockAbContext = new Mock<IAppDbContext>();
            var participates = new List<Nomination>().AsQueryable(); 
            var mockDbSet = new Mock<DbSet<Nomination>>();

            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Nomination>>().Setup(c => c.Expression).Returns(participates.Expression);

            mockAbContext.Setup(c => c.Nominations).Returns(mockDbSet.Object);
            var target = new ManagerRepository(mockAbContext.Object);

            // Act
            int count = target.ManagerCountForParticipate(managerId, topicId, trainerId);

            // Assert
            Assert.Equal(0, count);
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Nomination>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Nominations, Times.Once);
        }


        [Fact]
        public void GetParticipantByManagerId_ReturnsParticipant_WhenParticipantExists()
        {
            // Arrange
            int ParticipantId = 1;
            var Participant = new Participate
            {
                ParticipateId = 1,
                LastName = "test",
                FirstName = "test",
                Email = "S@gmail.com",
                JobId = 1,
                UserId = 1
            };
            var Participant1 = new Participate
            {
                ParticipateId = 2,
                LastName = "test",
                FirstName = "test",
                Email = "S@gmail.com",
                JobId = 1,
                UserId = 1
            };
            var participants = new List<Participate> { Participant,Participant1 }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Participate>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(participants.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(participants.Expression);
            mockAppDbContext.Setup(c => c.Participates).Returns(mockDbSet.Object);

            var target = new ManagerRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetParticipatesByManagerId(ParticipantId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(participants, actual);
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Participates, Times.Once);
        }

        [Fact]
        public void GetParticipantByManagerId_ReturnsNullParticipant_WhenNoParticipantBNotExists()
        {
            // Arrange
            int ParticipantId = 1;

            var participates = new List<Participate>().AsQueryable();

            var mockDbSet = new Mock<DbSet<Participate>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Provider).Returns(participates.Provider);
            mockDbSet.As<IQueryable<Participate>>().Setup(c => c.Expression).Returns(participates.Expression);
            mockAppDbContext.Setup(c => c.Participates).Returns(mockDbSet.Object);

            var target = new ManagerRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetParticipatesByManagerId(ParticipantId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(participates.Count(),actual.Count());
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Participate>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Participates, Times.Once);
        }



        [Fact]
        public void GetModeofTrainingByTopicId_WhenExists()
        {
            //Arrange
            int topicId = 1;
            int userId = 1;
            string modeOfTraining = "online";

            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.GetModeofTrainingByTopicId(userId,topicId)).Returns(modeOfTraining);

            var target = new ManagerRepository(mockAbContext.Object);

            //Act
            var actual = target.GetModeofTrainingByTopicId(userId,topicId);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(modeOfTraining, actual);
            mockAbContext.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId),Times.Once);
        }

        [Fact]
        public void GetModeofTrainingByTopicId_WhenNotExists()
        {
            //Arrange
            int topicId = 1;
            int userId = 1;

            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ManagerRepository(mockAbContext.Object);

            mockAbContext.Setup(c => c.GetModeofTrainingByTopicId(userId,topicId)).Returns("");
            //Act
            var actual = target.GetModeofTrainingByTopicId(userId, topicId);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal("", actual);
            mockAbContext.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId), Times.Once);

        }
    }
}
