using Microsoft.AspNetCore.Mvc;
using Moq;

using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Controllers;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Services.Contract;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPITests.Controllers
{
    public class AdminControllerTest : IDisposable
    {
        private readonly Mock<IAdminService> mockAdminService;

        public AdminControllerTest()
        {
            mockAdminService = new Mock<IAdminService>();
        }

        //-----MonthlyAdminReport-----------

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void MonthlyAdminReport_ReturnsOk_WhenDataExists()
        {

            //Arrange
            int userId = 1;
            int month = 6;
            int year = 2024;

            var expectedResponse = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>
            {
                Success = true,
            };

            mockAdminService.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(expectedResponse);

            var target = new AdminController(mockAdminService.Object);

            //Act

            var actual = target.MonthlyAdminReport(userId, month, year) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(expectedResponse, actual.Value);
            mockAdminService.Verify(c => c.MonthlyAdminReport(userId, month, year),Times.Once());

        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void MonthlyAdminReport_ReturnsNotFound_WhenNoDataExists()
        {
            //Arrange
            int userId = 1;
            int month = 6;
            int year = 2024;

            var expectedResponse = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>
            {
                Success = false,
                Data = new List<MonthlyAdminReportDto>()
            };

            mockAdminService.Setup(c => c.MonthlyAdminReport(userId, month, year)).Returns(expectedResponse);
            
            var target = new AdminController(mockAdminService.Object);
            
            //Act
            var actual = target.MonthlyAdminReport(userId, month, year) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(expectedResponse, actual.Value);
            mockAdminService.Verify(c => c.MonthlyAdminReport(userId, month, year), Times.Once);
        }


        ////------------DaterangeBasedReport--------------

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void DaterangeBasedReport_ReturnsOk_WhenDataExists()
        {
            //Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-01-01");
            DateTime endDate = DateTime.Parse("2024-06-01");

            var response = new ServiceResponse<IEnumerable<DaterangeBasedReportDto>>
            {
                Success = true,
            };

            mockAdminService.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(response);
            var target = new AdminController(mockAdminService.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }

        [Fact]

        [Trait("Admin", "AdminControllerTests")]
        [Trait("Admin", "AdminControllerTests")]
        public void DaterangeBasedReport_ReturnsNotFound_WhenNoDataExists()
        {
            // Arrange
            int jobId = 1;
            DateTime startDate = DateTime.Parse("2024-01-01");
            DateTime endDate = DateTime.Parse("2024-06-01");
            var response = new ServiceResponse<IEnumerable<DaterangeBasedReportDto>>
            {
                Success = false,
                Data = new List<DaterangeBasedReportDto>()
            };

            mockAdminService.Setup(c => c.DaterangeBasedReport(jobId, startDate, endDate)).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            //Act
            var actual = target.DaterangeBasedReport(jobId, startDate, endDate) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.DaterangeBasedReport(jobId, startDate, endDate), Times.Once);
        }



        //--------------Add Trainer--------------
        [Fact]
        public void AddTrainer_ReturnsOk_WhenTrainerAddedSuccessfully()
        {

            //Arrange
            var AddTrainerDto = new AddUserDto()
            {
                FirstName = "test",
                LastName = "test",
                LoginId = "test",
                Email = "test",
                JobId = 1
            };

            var responseString = new ServiceResponse<string>
            {
                Success = true,
                Message = "Trainer added successfully."
            };

            var target = new AdminController(mockAdminService.Object);

            mockAdminService.Setup(c => c.AddUser(It.IsAny<AddUserDto>())).Returns(responseString);
            //Act
            var actual = target.AddTrainer(AddTrainerDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(responseString, actual.Value);
            mockAdminService.Verify(c => c.AddUser(It.IsAny<AddUserDto>()), Times.Once);
        }

        [Theory]
        [Trait("Admin", "AdminControllerTests")]
        [InlineData("Trainer already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void AddTrainer_ReturnsNotFound_WhenTrainernotAdded(string errorMessage)
        {

            //Arrange
            var AddTrainerDto = new AddUserDto()
            {
                FirstName = "test",
                LastName = "test",
                LoginId = "test",
                Email = "test",
                JobId = 1
            };


            var responseString = new ServiceResponse<string>
            {
                Success = false,
                /* Data = AddTrainerDto */// Convert to TrainerDto
                Message = errorMessage
            };
            var target = new AdminController(mockAdminService.Object);

            mockAdminService.Setup(c => c.AddUser(It.IsAny<AddUserDto>())).Returns(responseString);
            //Act
            var actual = target.AddTrainer(AddTrainerDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(responseString, actual.Value);
            mockAdminService.Verify(c => c.AddUser(It.IsAny<AddUserDto>()), Times.Once);
        }

        //---------------All Trainers with pagination
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetJobs_returnsOk_WhenJobListExist()
        {
            //Arrange
            var expectedJobsList = new List<AllJobsDto>()
            {
               new AllJobsDto
               {
                   JobId = 1,
                   JobName = "test"
               },
               new AllJobsDto
               {
                   JobName = "hello",
                   JobId = 2
               }

            };

            var expectedServiceResponse = new ServiceResponse<IEnumerable<AllJobsDto>>()
            {
                Success = true,
                Data = expectedJobsList
            };
            mockAdminService.Setup(service => service.getAllJobs()).Returns(expectedServiceResponse);

            var target = new AdminController(mockAdminService.Object);

            //Act
            var actual = target.GetAllJobs() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(expectedServiceResponse, actual.Value);
            mockAdminService.Verify(c => c.getAllJobs(), Times.Once);
        }

        [Theory]
        [Trait("Admin", "AdminControllerTests")]
        [InlineData("No record found")]
        public void GetPaginatedTrainers_returnsbadRequest_WhenTrainerListExist(string errorMessage)
        {
            //Arrange

            var expectedServiceResponse = new ServiceResponse<IEnumerable<AllJobsDto>>()
            {
                Success = false,
                Message = errorMessage

            };
        
            mockAdminService.Setup(service => service.getAllJobs()).Returns(expectedServiceResponse);

            var target = new AdminController(mockAdminService.Object);

            //Act
            var actual = target.GetAllJobs() as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(expectedServiceResponse, actual.Value);
            mockAdminService.Verify(c => c.getAllJobs(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTrainerById_ReturnsOkWithTrainer_WhenTrainerExists()
        {
            //Arrange

            var Trainer = new UserDto()
            {
                FirstName = "test",
                LastName = "test",
                LoginId = "test",
                Email = "test",
                JobId = 1
            };

            var response = new ServiceResponse<UserDto>
            {
                Success = true,
                Data = Trainer // Convert to TrainerDto
            };

            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetTrainerByLoginId("test")).Returns(response);

            //Act
            var actual = target.GetTrainerByLoginId("test") as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetTrainerByLoginId("test"), Times.Once);
        }

        [Fact]
        public void GetTrainerById_ReturnsNotFoundWithTrainer_WhenTrainerNotExists()
        {
            //Arrange
            var response = new ServiceResponse<UserDto>
            {
                Success = false,

            };

            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetTrainerByLoginId("test")).Returns(response);

            //Act
            var actual = target.GetTrainerByLoginId("test") as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetTrainerByLoginId("test"), Times.Once);
        }


        //---------Get All Trainer with pagination------------
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllTrainerPagination_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int page = 1;
            int page_size = 10;

            var trainers = new List<TrainerDto>()
            {
                new TrainerDto()
                {
                    FirstName = "TestFirst1",
                    LastName = "TestLast1",
                    JobId = 1,

                },
                new TrainerDto()
                {
                    FirstName = "TestFirst1",
                    LastName = "TestLast1",
                    JobId = 1,

                },
            };

            var response = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Data = trainers,
                Success = true,
                Message = "",
            };

            mockAdminService.Setup(c => c.GetAllTrainerByPagination(page, page_size)).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetAllTraineryPagination(page, page_size) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllTrainerByPagination(page, page_size), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllTrainerPagination_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int page = 1;
            int page_size = 10;

            var response = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Success = false,
                Message = "",
            };

            mockAdminService.Setup(c => c.GetAllTrainerByPagination(page, page_size)).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetAllTraineryPagination(page, page_size) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllTrainerByPagination(page, page_size), Times.Once);
        }

        //-----------------Get Total Contacts----------------
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalTrainer_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var response = new ServiceResponse<int>()
            {
                Data = 10,
                Success = true,
                Message = "",
            };

            mockAdminService.Setup(c => c.TotalTrainer()).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetTotalCountOfPositions() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.TotalTrainer(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalTrainers_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange


            var response = new ServiceResponse<int>()
            {
                Data = 0,
                Success = false,
                Message = "",
            };


            mockAdminService.Setup(c => c.TotalTrainer()).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetTotalCountOfPositions() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.TotalTrainer(), Times.Once);
        }

        // --------------- AssignTopicToTrainer ---------------
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AssignTopicToTrainer_ReturnsOkResponse_WhenAssignedSuccessfully()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Assigned successfully"
            };

            mockAdminService.Setup(c => c.AssignTopicToTrainer(assignDto)).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.AssignTopicToTrainer(assignDto), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AssignTopicToTrainer_ReturnsBadRequestResponse_WhenNotAssignedSuccessfully()
        {
            // Arrange
            var assignDto = new AssignTrainingTopicDto()
            {
                UserId = 1,
                TopicId = 1,
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };
            var mockAdminService = new Mock<IAdminService>();
            mockAdminService.Setup(c => c.AssignTopicToTrainer(assignDto)).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.AssignTopicToTrainer(assignDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.AssignTopicToTrainer(assignDto), Times.Once);
        }



        //---------GetAllTrainer------------
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllTrainer_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange


            var trainers = new List<TrainerDto>()
            {
                new TrainerDto()
                {
                    UserId = 5,
                    FirstName = "TestFirst1",
                    LastName = "TestLast1",
                    JobId = 1,

                },
                new TrainerDto()
                {
                    UserId = 4,
                    FirstName = "TestFirst2",
                    LastName = "TestLast2",
                    JobId = 1,

                },
            };

            var response = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Data = trainers,
                Success = true,
                Message = "",
            };

            var mockAdminService = new Mock<IAdminService>();
            mockAdminService.Setup(c => c.GetAllTrainer()).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetAllTrainer() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllTrainer(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllTrainer_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange

            var response = new ServiceResponse<IEnumerable<TrainerDto>>()
            {
                Success = false,
                Message = "",
            };

            var mockAdminService = new Mock<IAdminService>();
            mockAdminService.Setup(c => c.GetAllTrainer()).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.GetAllTrainer() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllTrainer(), Times.Once);
        }

        // UnassignTopic
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void UnassignTopic_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Topic unassigned successfully",
            };

            mockAdminService.Setup(c => c.UnassignTopic(userId, topicId)).Returns(expectedResponse);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse, actual.Value);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAdminService.Verify(c => c.UnassignTopic(userId, topicId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void UnassignTopic_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            int userId = 1;
            int topicId = 1;

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time",
            };

            mockAdminService.Setup(c => c.UnassignTopic(userId, topicId)).Returns(expectedResponse);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.UnassignTopic(userId, topicId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse, actual.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAdminService.Verify(c => c.UnassignTopic(userId, topicId), Times.Once);
        }

        public void Dispose()
        {
            mockAdminService.VerifyAll();
        }
    }
}
