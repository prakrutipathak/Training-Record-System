using AutoFixture;
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
    public class AdminRepositoryTest : IDisposable
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;

        public AdminRepositoryTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();
        }

        //--------------------MonthlyAdminReport--------

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void MonthlyAdminReport_WhenExists()
        {
            //Arrange
            int userId = 1;
            int month = 6;
            int year = 2024;
            var expectedResults = new List<MonthlyAdminReportDto>
             {
                new MonthlyAdminReportDto
                {

                    TopicName = "Web Development",
                    StartDate =  DateTime.Parse("2024-01-01"),
                    EndDate =  DateTime.Parse("2024-06-01"),
                    Duration = 6,
                    ModePreference = "online",
                    TotalParticipateNo = 3

                },
                new MonthlyAdminReportDto
                {
                    TopicName = "Testing",
                    StartDate =  DateTime.Parse("2024-02-01"),
                    EndDate =  DateTime.Parse("2024-04-01"),
                    Duration = 2,
                    ModePreference = "offline",
                    TotalParticipateNo = 10


                }
             }.AsQueryable();

            mockAppDbContext.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(expectedResults);

            var target = new AdminRepository(mockAppDbContext.Object);
            
            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);
            
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResults.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void MonthlyAdminReport_WhenNotExists()
        {
            //Arrange
            int userId = 1;
            int month = 6;
            int year = 2024;
            var expectedResults = new List<MonthlyAdminReportDto>().AsQueryable();

            mockAppDbContext.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(expectedResults);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResults.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }

        ////-------------DaterangeBasedReport---------

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void DaterangeBasedReport_WhenExists()
        {
            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-01-01");
            DateTime endDate = DateTime.Parse("2024-06-01");
            var expectedResults = new List<DaterangeBasedReportDto>
             {
                 new DaterangeBasedReportDto
                {
                    TopicName = "Web Development",
                    TrainerName = "Trainer name 1",
                    StartDate =  DateTime.Parse("2024-01-01"),
                    EndDate =  DateTime.Parse("2024-06-01"),
                    Duration = 6,
                    ModePreference = "online",
                    TotalParticipateNo = 3

                },
                new DaterangeBasedReportDto
                {
                    TopicName = "Testing",
                    TrainerName = "Trainer name ",
                    StartDate =  DateTime.Parse("2024-02-01"),
                    EndDate =  DateTime.Parse("2024-04-01"),
                    Duration = 2,
                    ModePreference = "offline",
                    TotalParticipateNo = 10
                }
             }.AsQueryable();

            mockAppDbContext.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(expectedResults);
            var target = new AdminRepository(mockAppDbContext.Object);
            
            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);
            
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResults.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void DaterangeBasedReport_WhenNotExists()
        {
            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-01-01");
            DateTime endDate = DateTime.Parse("2024-06-01");
            var expectedResults = new List<DaterangeBasedReportDto>().AsQueryable();

            mockAppDbContext.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(expectedResults);
            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResults.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        //-------------------InsertTrainer----------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void InsertTrainer_ReturnsTrue()
        {
            //Arrange
            var users = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.InsertUser(users);

            //Assert
            Assert.True(actual);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);

        }
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void InsertTrainer_ReturnsFalse()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.InsertUser(null);

            //Assert
            Assert.False(actual);

        }

        //-----------Trainer exist---------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TrainerExists_ReturnsTrue()
        {
            // Arrange
            var name = "Test@gmail.com";
            var Trainers = new List<User>
      {
          new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          },
           new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          }
      }.AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(Trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(Trainers.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(name);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TrainerExists_ReturnsFalse()
        {
            // Arrange
            var name = "Trainer 3";
            var Trainers = new List<User>
      {
          new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          },
           new User()
          {
                  FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          }
      }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(Trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(Trainers.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(name);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }
        //-----------Trainer exist with id---------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TrainerExistsWithId_ReturnsTrue()
        {
            // Arrange
            var id = 2;
            var name = "Test@gmail.com";
            var Trainers = new List<User>
      {
          new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          },
           new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          }
      }.AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(Trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(Trainers.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(id, name);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TrainerExistsWithId_ReturnsFalse()
        {
            // Arrange
            var id = 1;
            var name = "Trainer 3";
            var Trainers = new List<User>
      {
          new User()
          {
              FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          },
           new User()
          {
                  FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                Loginbit = true,
                JobId = 1,
                LoginId = "Test",
                Role = 2,
          }
      }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(Trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(Trainers.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(id, name);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetJobs_ReturnsCorrectJobs_WhenJobsExists()
        {
            //Arrange
            var Jobs = new List<Job>
            {
                  new Job()
                  {
                      JobName = "Test 1",
                      JobId = 1,

                  },
                   new Job()
                  {
                      JobName = "Test 2",
                      JobId = 2,

                  },
                   new Job()
                  {
                      JobName = "Test 3",
                      JobId = 1,

                  },
                   new Job()
                  {
                      JobName = "Test 4",
                      JobId = 2,

                  }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Job>>();


            mockDbSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(Jobs.Provider);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(Jobs.Expression);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(Jobs.ElementType);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(Jobs.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Jobs).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetAllJobs();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(3, actual.Count());
            mockAppDbContext.Verify(c => c.Jobs, Times.AtLeastOnce);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetJobs_ReturnsCorrectJobs_WhenJobsNotExists()
        {
            //Arrange
            var Jobs = new List<Job> { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Job>>();

            mockDbSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(Jobs.Provider);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(Jobs.Expression);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(Jobs.ElementType);
            mockDbSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(Jobs.GetEnumerator());


            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Jobs).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetAllJobs();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(Jobs.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.Jobs, Times.AtLeastOnce);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Job>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        //--------------Get Trainer by id----------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetTrainer_ReturnsTrainer_WhenTrainerExists()
        {
            // Arrange
            string TrainerId = "test";
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Loginbit = true,
                LoginId = "test"

            };
            var categories = new List<User> { Trainer }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(categories.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(categories.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainerDetailsByLoginId(TrainerId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(Trainer, actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetTrainer_ReturnsNullTrainer_WhenNoTrainerExists()
        {
            // Arrange
            string TrainerId = "Test";

            var categories = new List<User>().AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(categories.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(categories.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetTrainerDetailsByLoginId(TrainerId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        //-----------Get All Trainer By Pagination -------------

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetPaginatedTrainers_ReturnsCorrectTrainers_WithPagination()
        {
            // Arrange
            var trainers = new List<User>
            {
                new User { UserId = 1, FirstName = "firstName1", LastName = "lastName1", Email = "test1@gmail.com", Role=2, Job = new Job() },
                new User { UserId = 2, FirstName = "firstName2", LastName = "lastName2", Email = "test2@gmail.com", Role=2,Job = new Job() },
                new User { UserId = 3, FirstName = "firstName3", LastName = "lastName3", Email = "test3@gmail.com", Role=2,Job = new Job() },
                new User { UserId = 4, FirstName = "firstName4", LastName = "lastName4", Email = "test4@gmail.com", Role=2,Job = new Job()}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(trainers.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(trainers.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(trainers.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllTrainerPagination(1, 4);

            // Assert
            Assert.Equal(4, actual.Count());
            Assert.Equal("firstName1", actual.First().FirstName);
            Assert.Equal("firstName4", actual.Last().FirstName);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<User>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        //-----------Total Trainer count----------------

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetTotalTrainer_ReturnTrainer()
        {
            //Arrange
            var trainers = new List<User>
            {
                new User { UserId = 1, FirstName = "firstName1", LastName = "lastName1", Email = "test1@gmail.com", Role=2, Job = new Job() },
                new User { UserId = 2, FirstName = "firstName2", LastName = "lastName2", Email = "test2@gmail.com", Role=2,Job = new Job() },
                new User { UserId = 3, FirstName = "firstName3", LastName = "lastName3", Email = "test3@gmail.com", Role=2,Job = new Job() },
                new User { UserId = 4, FirstName = "firstName4", LastName = "lastName4", Email = "test4@gmail.com", Role=2,Job = new Job()}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(trainers.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(trainers.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(trainers.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);
            //Act
            var actual = target.TotalNoOfTrainer();

            //Assert
            Assert.Equal(4, actual);

            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<User>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        //---------GetAllTrainer-------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetAllTrainer_ReturnsCorrectTrainers_WhenTrainersExists()
        {
            //Arrange
            var trainers = new List<User>
            {
                new User { UserId = 1, FirstName = "firstName1", LastName = "lastName1", Email = "test1@gmail.com", Role=2  },
                new User { UserId = 2, FirstName = "firstName2", LastName = "lastName2", Email = "test2@gmail.com", Role=2},
                new User { UserId = 3, FirstName = "firstName3", LastName = "lastName3", Email = "test3@gmail.com", Role=2},
                new User { UserId = 4, FirstName = "firstName4", LastName = "lastName4", Email = "test4@gmail.com", Role=2}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(trainers.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(trainers.ElementType);

            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(trainers.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetAllTrainer();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(4, actual.Count());
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<User>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetAllTrainer_ReturnsCorrectTrainers_WhenTrainersNotExists()
        {
            //Arrange
            var trainers = new List<User> { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();


            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(trainers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(trainers.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(trainers.ElementType);

            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(trainers.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetAllTrainer();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(trainers.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.Users, Times.AtLeastOnce);

            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<User>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        // --------------- AssignTopicToTrainer -------------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void AssignTopicToTrainer_ReturnsTrue_WhenTopicAssignedSuccessfully()
        {
            // Arrange
            var trainerTopic = new TrainerTopic()
            {
                UserId = 1,
                TopicId = 1,
            };

            var mockDbSet = new Mock<DbSet<TrainerTopic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(c => c.TrainerTopics).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.AssignTopicToTrainer(trainerTopic);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(trainerTopic), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void AssignTopicToTrainer_ReturnsFalse_WhenTopicAssignmentFails()
        {
            // Arrange
            TrainerTopic trainerTopic = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.AssignTopicToTrainer(trainerTopic);

            // Assert
            Assert.False(actual);
        }

        // -------------------- GetUserDetails ----------------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetUserDetails_ReturnsUser_WhenUserExists()
        {
            // Arrange
            int userId = 10;

            var user = new User()
            {
                UserId = userId,
                FirstName = "test",
                LastName = "test",
                LoginId = "test",
            };

            var users = new List<User>()
            {
                user,
                new User()
                {
                    UserId = 11,
                    FirstName = "test 2",
                    LastName = "test 2",
                    LoginId = "test2",
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserDetails(userId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void GetUserDetails_ReturnsEmpty_WhenUserDoesntExists()
        {
            // Arrange
            int userId = 10;

            var user = new User()
            {
                UserId = 12,
                FirstName = "test",
                LastName = "test",
                LoginId = "test",
            };

            var users = new List<User>()
            {
                user,
                new User()
                {
                    UserId = 11,
                    FirstName = "test 2",
                    LastName = "test 2",
                    LoginId = "test2",
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserDetails(userId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        // ------------------- TopicAlreadyAssigned -------------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TopicAlreadyAssigned_ReturnsTrue_WhenAlreadyAssigned()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            var topic = new TrainerTopic()
            {
                TrainerTopicId = 1,
                TopicId = topicId,
                UserId = 1,
            };

            var topics = new List<TrainerTopic>()
            {
                topic,
                new TrainerTopic()
                {
                    TrainerTopicId = 2,
                    TopicId = 2,
                    UserId = 2,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerTopic>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.TrainerTopics).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TopicAlreadyAssigned(userId, topicId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerTopics, Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void TopicAlreadyAssigned_ReturnsFalse_WhenTopicIsNotAssigned()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            var topic = new TrainerTopic()
            {
                TrainerTopicId = 1,
                TopicId = 3,
                UserId = 1,
            };

            var topics = new List<TrainerTopic>()
            {
                topic,
                new TrainerTopic()
                {
                    TrainerTopicId = 2,
                    TopicId = 2,
                    UserId = 2,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TrainerTopic>>();
            var mockAppDbContext = new Mock<IAppDbContext>(); 

            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(topics.Provider);
            mockDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(topics.Expression);
            mockAppDbContext.Setup(c => c.TrainerTopics).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TopicAlreadyAssigned(userId, topicId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.TrainerTopics, Times.Once);
        }

        // ------------------ UnassignTopic ----------------------------
        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void UnassignTopic_ReturnsTrue_WhenTopicUnassignedSuccessfullyFromBothTables()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;
            int trainerTopicId = 1;

            var trainerTopic = new TrainerTopic()
            {
                TrainerTopicId = trainerTopicId,
                TopicId = topicId,
                UserId = userId,
            };

            var trainerTopics = new List<TrainerTopic>()
            {
                trainerTopic,
                new TrainerTopic()
                {
                    TrainerTopicId = 2,
                    TopicId = 2,
                    UserId = userId,
                }
            }.AsQueryable();

            var trainerProgramDetail = new TrainerProgramDetail()
            {
                TrainerProgramDetailId = 1,
                TrainerTopicId = 1,
            };

            var trainerProgramDetails = new List<TrainerProgramDetail>()
            {
                trainerProgramDetail,
                new TrainerProgramDetail()
                {
                    TrainerProgramDetailId = 2,
                    TrainerTopicId = 2,
                }
            }.AsQueryable();

            var mockTrainerTopicsDbSet = new Mock<DbSet<TrainerTopic>>();
            var mockDetailsDbSet = new Mock<DbSet<TrainerProgramDetail>>();

            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(trainerTopics.Expression);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(trainerTopics.Provider);

            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(trainerProgramDetails.Expression);
            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(trainerProgramDetails.Provider);
            
            mockAppDbContext.SetupGet(c => c.TrainerTopics).Returns(mockTrainerTopicsDbSet.Object);
            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDetailsDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId);

            // Assert
            Assert.True(actual);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Once);

            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);

            mockAppDbContext.VerifyGet(c => c.TrainerTopics, Times.Exactly(2));
            mockAppDbContext.VerifyGet(c => c.TrainerProgramDetails, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepository")]
        public void UnassignTopic_ReturnsTrue_WhenTopicIsUnassignedFromOneTable()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;
            int trainerTopicId = 1;

            var trainerTopic = new TrainerTopic()
            {
                TrainerTopicId = trainerTopicId,
                TopicId = topicId,
                UserId = userId,
            };

            var trainerTopics = new List<TrainerTopic>()
            {
                trainerTopic,
                new TrainerTopic()
                {
                    TrainerTopicId = 2,
                    TopicId = 2,
                    UserId = userId,
                }
            }.AsQueryable();

            var trainerProgramDetail = new TrainerProgramDetail()
            {
                TrainerProgramDetailId = 3,
                TrainerTopicId = 3,
            };

            var trainerProgramDetails = new List<TrainerProgramDetail>()
            {
                trainerProgramDetail,
                new TrainerProgramDetail()
                {
                    TrainerProgramDetailId = 2,
                    TrainerTopicId = 2,
                }
            }.AsQueryable();

            var mockTrainerTopicsDbSet = new Mock<DbSet<TrainerTopic>>();
            var mockDetailsDbSet = new Mock<DbSet<TrainerProgramDetail>>();

            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Expression).Returns(trainerTopics.Expression);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Setup(c => c.Provider).Returns(trainerTopics.Provider);

            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Expression).Returns(trainerProgramDetails.Expression);
            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Setup(c => c.Provider).Returns(trainerProgramDetails.Provider);

            mockAppDbContext.SetupGet(c => c.TrainerTopics).Returns(mockTrainerTopicsDbSet.Object);
            mockAppDbContext.SetupGet(c => c.TrainerProgramDetails).Returns(mockDetailsDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId);

            // Assert
            Assert.True(actual);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Expression, Times.Once);
            mockTrainerTopicsDbSet.As<IQueryable<TrainerTopic>>().Verify(c => c.Provider, Times.Once);

            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Expression, Times.Once);
            mockDetailsDbSet.As<IQueryable<TrainerProgramDetail>>().Verify(c => c.Provider, Times.Once);

            mockAppDbContext.VerifyGet(c => c.TrainerTopics, Times.Exactly(2));
            mockAppDbContext.VerifyGet(c => c.TrainerProgramDetails, Times.Exactly(1));
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        public void Dispose()
        {
            mockAppDbContext.VerifyAll();
        }
    }
}
