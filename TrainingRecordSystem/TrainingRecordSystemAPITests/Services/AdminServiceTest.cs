using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPITests.Services
{
    public class AdminServiceTest : IDisposable
    {
        private readonly Mock<IAdminRepository> mockAdminRepository;
        private readonly Mock<ITopicRepository> mockTopicRepository;
        private readonly Mock<IPasswordService> mockPasswordService;

        public AdminServiceTest()
        {
            mockAdminRepository = new Mock<IAdminRepository>();
            mockTopicRepository = new Mock<ITopicRepository>();
            mockPasswordService = new Mock<IPasswordService>();
        }

        //----------------MonthlyAdminReport---------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void MonthlyAdminReport_ReturnList_WhenNoDataExist()
        {
            //Arrange
            int userId = 1;
            int month = 12;
            int year = 2001;
            var mockReports = new List<MonthlyAdminReportDto>();
            var expectedResponse = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>()
            {
                Success = false,
                Message = "No record found"
            };
            
            mockAdminRepository.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void MonthlyAdminReport_ReturnsList_WhenDataExist()
        {

            //Arrange
            int userId = 1;
            int month = 12;
            int year = 2001;
            var mockReports = new List<MonthlyAdminReportDto>()
            {
                new MonthlyAdminReportDto()
                {
                    TopicName = "Topic 1",
                    StartDate = DateTime.Parse("2024-07-24"),
                    EndDate = DateTime.Parse("2024-08-25"),
                    Duration = 1,
                    ModePreference = "offline",
                    TotalParticipateNo = 4,
                },
                new MonthlyAdminReportDto()
                {
                    TopicName = "Topic 2",
                    StartDate = DateTime.Parse("2024-07-26"),
                    EndDate = DateTime.Parse("2024-08-27"),
                    Duration = 2,
                    ModePreference = "online",
                    TotalParticipateNo = 5,
                },
            };
            var expectedResponse = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>()
            {
                Success = true,
                Message = "Success"
            };

            mockAdminRepository.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.True(actual.Success);
            Assert.Equal(mockReports.Count, actual.Data.Count());
            mockAdminRepository.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);

            for (int i = 0; i < mockReports.Count; i++)
            {
                Assert.Equal(mockReports[i].TopicName, actual.Data.ElementAt(i).TopicName);
                Assert.Equal(mockReports[i].StartDate, actual.Data.ElementAt(i).StartDate);
                Assert.Equal(mockReports[i].EndDate, actual.Data.ElementAt(i).EndDate);
                Assert.Equal(mockReports[i].Duration, actual.Data.ElementAt(i).Duration);
                Assert.Equal(mockReports[i].ModePreference, actual.Data.ElementAt(i).ModePreference);
                Assert.Equal(mockReports[i].TotalParticipateNo, actual.Data.ElementAt(i).TotalParticipateNo);
            }
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void MonthlyAdminReport_ReturnsFalse_WhenReportsAreNull()
        {
            //Arrange
            int userId = 1;
            int month = 12;
            int year = 2001;
            List<MonthlyAdminReportDto> mockReports = null;
            var expectedResponse = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>()
            {
                Success = false,
                Message = "No record found"
            };

            mockAdminRepository.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void MonthlyAdminReport_WhenThrowException()
        {
            //Arrange
            int userId = 1;
            int month = 12;
            int year = 2001;
           
            mockAdminRepository.Setup(c => c.MonthlyAdminReport(userId, month, year)).Throws(new Exception());

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.MonthlyAdminReport(userId, month, year);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }


        ////----------------DaterangeBasedReport----------

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DaterangeBasedReport_ReturnList_WhenNoDataExist()
        {
            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-07-27");
            DateTime endDate = DateTime.Parse("2024-08-27");
            var mockReports = new List<DaterangeBasedReportDto>();
            var expectedResponse = new ServiceResponse<List<DaterangeBasedReportDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            mockAdminRepository.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DaterangeBasedReport_ReturnsFalse_WhenNoDataExists()
        {
            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-07-27");
            DateTime endDate = DateTime.Parse("2024-08-27");
            List<DaterangeBasedReportDto> mockReports = null;
            var expectedResponse = new ServiceResponse<List<DaterangeBasedReportDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            mockAdminRepository.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DaterangeBasedReport_ReturnsList_WhenDataExist()
        {

            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-07-27");
            DateTime endDate = DateTime.Parse("2024-08-27");
            var mockReports = new List<DaterangeBasedReportDto>()
            {
                new DaterangeBasedReportDto()
                {
                    TopicName = "Topic 1",
                    TrainerName = "Trainer 1",
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = 3,
                    ModePreference = "offline",
                    TotalParticipateNo = 5
                },
                new DaterangeBasedReportDto()
                {
                    TopicName = "Topic 2",
                    TrainerName = "Trainer 2",
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = 2,
                    ModePreference = "offline",
                    TotalParticipateNo = 5
                },
            };
            var expectedResponse = new ServiceResponse<List<DaterangeBasedReportDto>>()
            {
                Data = mockReports,
                Success = true,
                Message = "Success",
            };

            mockAdminRepository.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(mockReports);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.True(actual.Success);
            mockAdminRepository.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);

            for (int i = 0; i < mockReports.Count; i++)
            {
                Assert.Equal(mockReports[i].TopicName, actual.Data.ElementAt(i).TopicName);
                Assert.Equal(mockReports[i].TrainerName, actual.Data.ElementAt(i).TrainerName);
                Assert.Equal(mockReports[i].StartDate, actual.Data.ElementAt(i).StartDate);
                Assert.Equal(mockReports[i].EndDate, actual.Data.ElementAt(i).EndDate);
                Assert.Equal(mockReports[i].Duration, actual.Data.ElementAt(i).Duration);
                Assert.Equal(mockReports[i].ModePreference, actual.Data.ElementAt(i).ModePreference);
                Assert.Equal(mockReports[i].TotalParticipateNo, actual.Data.ElementAt(i).TotalParticipateNo);
            }
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DaterangeBasedReport_WhenThrowException()
        {

            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-07-27");
            DateTime endDate = DateTime.Parse("2024-08-27");
          
            mockAdminRepository.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Throws(new Exception());

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        //------------------Add Trainer----------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AddTrainer_ReturnSuccess_WhenTrainerAddedSuccessfully()
        {
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,

            };
            var addTrainerDto = new AddUserDto
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,
            };

            var serviceResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Trainer added successfully."
            };

            var mockAdminRepository = new Mock<IAdminRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockTopicRepository = new Mock<ITopicRepository>();



            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.UserExists(addTrainerDto.Email)).Returns(false);

            mockAdminRepository.Setup(c => c.InsertUser(It.IsAny<User>())).Returns(true);

            //Act

            var actual = target.AddUser(addTrainerDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(serviceResponse.Message, actual.Message);

            mockAdminRepository.Verify(c => c.UserExists(Trainer.Email), Times.Once);

            mockAdminRepository.Verify(c => c.InsertUser(Trainer), Times.Never);

        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AddTrainer_ReturnError_WhenTrainerNotAddedSuccessfully()
        {
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,

            };
            var addTrainerDto = new AddUserDto
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,
            };

            var serviceResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };

            var mockAdminRepository = new Mock<IAdminRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockTopicRepository = new Mock<ITopicRepository>();



            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.UserExists(Trainer.Email)).Returns(false);

            mockAdminRepository.Setup(c => c.InsertUser(It.IsAny<User>())).Returns(false);

            //Act

            var actual = target.AddUser(addTrainerDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(serviceResponse.Message, actual.Message);

            mockAdminRepository.Verify(c => c.UserExists(Trainer.Email), Times.Once);

            mockAdminRepository.Verify(c => c.InsertUser(Trainer), Times.Never);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AddTrainer_ReturnError_WhenTrainerAlreadyExists()
        {
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                LoginId = "Test",
                JobId = 1,

            };
            var addTrainerDto = new AddUserDto
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                LoginId = "Test",
                JobId = 1,
            };

            var serviceResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "User already exist"
            };

            var mockAdminRepository = new Mock<IAdminRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockTopicRepository = new Mock<ITopicRepository>();



            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);


            mockAdminRepository.Setup(c => c.UserExists(Trainer.Email)).Returns(true);

            //Act

            var actual = target.AddUser(addTrainerDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(serviceResponse.Message, actual.Message);

            mockAdminRepository.Verify(c => c.UserExists(Trainer.Email), Times.Once);


        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AddTrainer_WhenThrowException()
        {
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                LoginId = "Test",
                JobId = 1,

            };
            var addTrainerDto = new AddUserDto
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                LoginId = "Test",
                JobId = 1,
            };


            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);
            mockAdminRepository.Setup(c => c.UserExists(Trainer.Email)).Throws(new Exception());

            //Act

            var actual = target.AddUser(addTrainerDto);

            //Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.UserExists(Trainer.Email), Times.Once);

        }

        //------------getalljobs
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobs_returnJobs_WhenJobsExist()
        {

            var expectedJobList = new List<Job>()
            {
                new Job {
                    JobId = 1,
                    JobName = "test"
                },
                new Job
                {
                    JobName = "test1",
                    JobId = 2,
                }
            };

            var JobList = new List<AllJobsDto>() {
                new AllJobsDto { JobId = 1,JobName = "test"},
                  new AllJobsDto { JobName = "test1", JobId = 2 }
            };

            var serviceResponse = new ServiceResponse<IEnumerable<AllJobsDto>>()
            {
                Success = true,
                Data = JobList
            };
          
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllJobs()).Returns(expectedJobList);

            //Act
            var actual = target.getAllJobs();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(JobList.Count(), actual.Data.Count());
            Assert.Equal(serviceResponse.Data.ToString(), actual.Data.ToString());
            mockAdminRepository.Verify(c => c.GetAllJobs(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobs_returnEmptyList_WhenTrainerNotExist()
        {

            var jobList = new List<AllJobsDto>() { };

            var serviceResponse = new ServiceResponse<IEnumerable<AllJobsDto>>()
            {
                Success = false,
                Data = jobList,
                Message = "No record found"
            };

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllJobs()).Returns<Job>(null);

            //Act
            var actual = target.getAllJobs();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(serviceResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllJobs(), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobs_WhenThrowException()
        {
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllJobs()).Throws(new Exception());

            //Act
            var actual = target.getAllJobs();

            //Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.GetAllJobs(), Times.Once);
        }

        //----------get trainer by loginId---------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetTrainer_ReturnsTrainerById_WhenTrainerExists()
        {
            var Trainer = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,
            };
            var TrainerDto = new UserDto
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@gmail.com",
                LoginId = "Test",
                JobId = 1,
            };
          
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetTrainerDetailsByLoginId("Test")).Returns(Trainer);
            // Act 
            var actual = target.GetTrainerByLoginId("Test");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(TrainerDto.ToString(), actual.Data.ToString());
            mockAdminRepository.Verify(c => c.GetTrainerDetailsByLoginId("Test"), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetTrainer_ReturnsErrorMeesssag_WhenTrainerisNull()
        {
            var response = new ServiceResponse<UserDto>
            {
                Success = false,
                Message = "No record found!"
            };
           
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetTrainerDetailsByLoginId("Test")).Returns<User>(null);

            // Act 
            var actual = target.GetTrainerByLoginId("Test");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetTrainerDetailsByLoginId("Test"), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetTrainer_ThrowException()
        {
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetTrainerDetailsByLoginId("Test")).Throws(new Exception());

            // Act 
            var actual = target.GetTrainerByLoginId("Test");

            // Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.GetTrainerDetailsByLoginId("Test"), Times.Once);
        }

        //---------------GetAllTrainerByPagination-----------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_ReturnErrorMessage_whenNoParticipantsExist()
        {
            //Arrange
            IEnumerable<User> trainer = null;
            var response = new ServiceResponse<IEnumerable<User>>()
            {
                Data = trainer,
                Success = false,
                Message = "No record found",
            };
         
            mockAdminRepository.Setup(c => c.GetAllTrainerPagination(1, 2)).Returns(trainer);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.GetAllTrainerByPagination(1, 2);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllTrainerPagination(1, 2), Times.Once);

        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_ReturnTrainer_whenTrainerExist()
        {
            //Arrange
            var trainer = new List<User>
            {
                new User { UserId = 1, FirstName = "firstName1", LastName = "lastName1", Email = "test1@gmail.com", Role=2, Job = new Job() }

            };
            var response = new ServiceResponse<IEnumerable<User>>()
            {
                Data = trainer,
                Success = true,
                Message = "Success",
            };
          
            mockAdminRepository.Setup(c => c.GetAllTrainerPagination(1, 2)).Returns(trainer);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.GetAllTrainerByPagination(1, 2);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllTrainerPagination(1, 2), Times.Once);

        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_ThrowException()
        {
            //Arrange
            mockAdminRepository.Setup(c => c.GetAllTrainerPagination(1, 2)).Throws(new Exception());
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.GetAllTrainerByPagination(1, 2);

            //Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.GetAllTrainerPagination(1, 2), Times.Once);

        }

        //--------------Total Trainer Count---------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void TotalTrainerCount_ReturnsResponse_WhenContactExists()
        {
            //Arrange
            var trainer = new List<User>
            {
                new User { UserId = 1, FirstName = "firstName1", LastName = "lastName1", Email = "test1@gmail.com", Role=2, Job = new Job() }

            };
           
            mockAdminRepository.Setup(c => c.TotalNoOfTrainer()).Returns(1);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.TotalTrainer();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(1, actual.Data);
            mockAdminRepository.Verify(c => c.TotalNoOfTrainer(), Times.Once);

        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void TotalTrainerCount_ReturnErrorMessage_whenNoTrainerExist()
        {
            //Arrange
            int trainers = 0;
            var response = new ServiceResponse<int>()
            {
                Data = trainers,
                Success = false,
                Message = "No record found",
            };
            mockAdminRepository.Setup(c => c.TotalNoOfTrainer()).Returns(trainers);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.TotalTrainer();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.TotalNoOfTrainer(), Times.Once);

        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void TotalTrainerCount_WhenThrowException()
        {
            //Arrange
          
            mockAdminRepository.Setup(c => c.TotalNoOfTrainer()).Throws(new Exception());
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            //Act
            var actual = target.TotalTrainer();

            //Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.TotalNoOfTrainer(), Times.Once);

        }

        // ---------------- Assign topic to trainer --------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsSuccess_WhenAssignedSuccessfully()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                JobId = 1,
            };
            var topic = new Topic()
            {
                TopicId = 1,
                TopicName = "Topic 1",
                JobId = 1,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Topic successfully assigned to trainer",
            };

            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns(user);
            mockTopicRepository.Setup(c => c.GetTopicDetails(assignDto.TopicId)).Returns(topic);
            mockAdminRepository.Setup(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId)).Returns(false);
            mockAdminRepository.Setup(c => c.AssignTopicToTrainer(It.IsAny<TrainerTopic>())).Returns(true);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
            mockTopicRepository.Verify(c => c.GetTopicDetails(assignDto.TopicId), Times.Once);
            mockAdminRepository.Verify(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId), Times.Once);
            mockAdminRepository.Verify(c => c.AssignTopicToTrainer(It.IsAny<TrainerTopic>()), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenNotAssignedSuccessfully()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                JobId = 1,
            };
            var topic = new Topic()
            {
                TopicId = 1,
                TopicName = "Topic 1",
                JobId = 1,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after sometime",
            };
            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns(user);
            mockTopicRepository.Setup(c => c.GetTopicDetails(assignDto.TopicId)).Returns(topic);
            mockAdminRepository.Setup(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId)).Returns(false);
            mockAdminRepository.Setup(c => c.AssignTopicToTrainer(It.IsAny<TrainerTopic>())).Returns(false);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
            mockTopicRepository.Verify(c => c.GetTopicDetails(assignDto.TopicId), Times.Once);
            mockAdminRepository.Verify(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId), Times.Once);
            mockAdminRepository.Verify(c => c.AssignTopicToTrainer(It.IsAny<TrainerTopic>()), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenTopicAlreadyAssigned()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                JobId = 1,
            };
            var topic = new Topic()
            {
                TopicId = 1,
                TopicName = "Topic 1",
                JobId = 1,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Topic has already been assigned",
            };
            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns(user);
            mockTopicRepository.Setup(c => c.GetTopicDetails(assignDto.TopicId)).Returns(topic);
            mockAdminRepository.Setup(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId)).Returns(true);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
            mockTopicRepository.Verify(c => c.GetTopicDetails(assignDto.TopicId), Times.Once);
            mockAdminRepository.Verify(c => c.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenJobIdDoesNotMatch()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                JobId = 1,
            };
            var topic = new Topic()
            {
                TopicId = 1,
                TopicName = "Topic 1",
                JobId = 2,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Cannot be assigned to trainer as job role doesn't match",
            };
            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns(user);
            mockTopicRepository.Setup(c => c.GetTopicDetails(assignDto.TopicId)).Returns(topic);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
            mockTopicRepository.Verify(c => c.GetTopicDetails(assignDto.TopicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenTopicDoesNotExist()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                JobId = 1,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Topic doesn't exist",
            };

            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns(user);
            mockTopicRepository.Setup(c => c.GetTopicDetails(assignDto.TopicId)).Returns<Topic>(null);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
            mockTopicRepository.Verify(c => c.GetTopicDetails(assignDto.TopicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenUserIsNull()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after sometime",
            };

            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Returns<User>(null);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ReturnsErrorMessage_WhenDtoIsNull()
        {
            // Arrange
            AssignTrainingTopicDto assignDto = null;

            var mockResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after sometime",
            };

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AssignTopicToTrainer_ThrowException()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };

            mockAdminRepository.Setup(c => c.GetUserDetails(assignDto.UserId)).Throws(new Exception());

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.GetUserDetails(assignDto.UserId), Times.Once);
        }



        //------------GetAllTrainer-----------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_returnTrainer_WhenJobsExist()
        {

            var expectedTrainerList = new List<User>()
            {
                new User {
                    UserId = 1,
                    LoginId = "loginid1",
                    FirstName = "first name 1",
                    LastName = "last name 1",
                    JobId = 1,
                     Job = new Job()
                     {
                            JobId = 1,
                            JobName = "Developer"
                     }

                },
                new User
                {
                    UserId = 2,
                    LoginId = "loginid2",
                    FirstName = "first name 2",
                    LastName = "last name 2",
                    JobId = 2,
                     Job = new Job()
                     {
                            JobId = 2,
                            JobName = "Tester"
                     }
                }
            };

            var TrainerList = new List<TrainerDto>() {
                new TrainerDto {
                    UserId = 1,
                    FirstName = "first name 1",
                    LastName = "last name 1",
                    JobId = 1,
                    Job = new Job()
                     {
                            JobId = 1,
                            JobName = "Developer"
                     }
                 },
                  new TrainerDto {
                    UserId = 2,
                    FirstName = "first name 2",
                    LastName = "last name 2",
                    JobId = 2,
                    Job = new Job()
                     {
                            JobId = 2,
                            JobName = "Tester"
                     }
                  }
            };

            var serviceResponse = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Success = true,
                Data = TrainerList
            };
          
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllTrainer()).Returns(expectedTrainerList);

            //Act
            var actual = target.GetAllTrainer();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(TrainerList.Count(), actual.Data.Count());
            Assert.Equal(serviceResponse.Data.ToString(), actual.Data.ToString());
            mockAdminRepository.Verify(c => c.GetAllTrainer(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_returnEmptyList_WhenTrainerNotExist()
        {
            var TrainerList = new List<TrainerDto>() { };

            var serviceResponse = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Success = false,
                Data = TrainerList,
                Message = "No record found"
            };

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllTrainer()).Returns<Job>(null);

            //Act
            var actual = target.GetAllTrainer();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(serviceResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllTrainer(), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllTrainer_WhenThrowException()
        {
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            mockAdminRepository.Setup(c => c.GetAllTrainer()).Throws(new Exception());

            //Act
            var actual = target.GetAllTrainer();

            //Assert
            Assert.NotNull(actual);
            mockAdminRepository.Verify(c => c.GetAllTrainer(), Times.Once);
        }

        //-------------- UnassignTopic ------------------
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UnassignTopic_ReturnsTrue_WhenTopicUnassignedSuccessfully()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;
            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Topic unassigned successfully"
            };
            mockAdminRepository.Setup(c => c.UnassignTopic(userId, topicId)).Returns(true);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAdminRepository.Verify(c => c.UnassignTopic(userId, topicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UnassignTopic_ReturnsFalse_WhenTopicUnassignmentFails()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time",
            };
            mockAdminRepository.Setup(c => c.UnassignTopic(userId, topicId)).Returns(false);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Message, actual.Message);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.UnassignTopic(userId, topicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UnassignTopic_ThrowException()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            mockAdminRepository.Setup(c => c.UnassignTopic(userId, topicId)).Throws(new Exception());

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object, mockTopicRepository.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockAdminRepository.Verify(c => c.UnassignTopic(userId, topicId), Times.Once);
        }

        public void Dispose()
        {
            mockAdminRepository.VerifyAll();
            mockTopicRepository.VerifyAll();
            mockPasswordService.VerifyAll();
        }
    }
}
