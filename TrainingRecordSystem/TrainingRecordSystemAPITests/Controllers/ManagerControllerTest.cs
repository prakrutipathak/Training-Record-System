using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Controllers;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPITests.Controllers
{
    public class ManagerControllerTest
    {

        //GetModeofTrainingByTopicId

        [Fact]
        public void GetModeofTrainingByTopicId_ReturnsOk_WhenModeOfTrainingExists()
        {
            //Arrange

            string modeOfTraining = "online";
            int topicId = 1;
            int userId = 1;

            var response = new ServiceResponse<string>
            {
                Success = true,
                Data = modeOfTraining
            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetModeofTrainingByTopicId(userId, topicId)).Returns(response);

            //Act
            var actual = target.GetModeofTrainingByTopicId(userId, topicId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId), Times.Once);
        }

        [Fact]
        public void GetModeofTrainingByTopicId_ReturnsNotFound_WhenModeOfTrainingExists()
        {
            //Arrange

            int topicId = 1;
            int userId = 1;

            var response = new ServiceResponse<string>
            {
                Success = false,
                Data = null
            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetModeofTrainingByTopicId(userId, topicId)).Returns(response);

            //Act
            var actual = target.GetModeofTrainingByTopicId(userId, topicId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId), Times.Once);
        }


        //UpcomingTrainingProgram
        [Fact]
        public void UpcomingTrainingProgram_ReturnsOk_WhenDataExists()
        {
            //Arrange
            int jobId = 1;
            var response = new ServiceResponse<IEnumerable<ManagerReport>>
            {
                Success = true,
            };

            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.UpcomingTrainingProgram(jobId)).Returns(response);

            //Act
            var actual = target.UpcomingTrainingProgram(jobId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.UpcomingTrainingProgram(jobId), Times.Once);
        }

        [Fact]
        public void UpcomingTrainingProgram_ReturnsNotFound_WhenNoDataExists()
        {
            int jobId = 1;
            //Arrange
            var response = new ServiceResponse<IEnumerable<ManagerReport>>
            {
                Success = false,
                Data = new List<ManagerReport>(),

            };

            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.UpcomingTrainingProgram(jobId)).Returns(response);

            //Act
            var actual = target.UpcomingTrainingProgram(jobId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.UpcomingTrainingProgram(jobId), Times.Once);
        }
        //AddParticipate
        [Fact]
        public void AddParticipate_ReturnsBadRequest_WhenModelIsInValid()
        {
            var fixture = new Fixture();
            var addParticipateDto = fixture.Create<AddParticipateDto>();
            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            target.ModelState.AddModelError("Email", "Email is required");
            //Act

            var actual = target.AddParticipate(addParticipateDto) as BadRequestResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.False(target.ModelState.IsValid);
        }


        [Fact]
        public void AddParticipate_ReturnsOk_WhenPartcipateIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addParticipateDto = fixture.Create<AddParticipateDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.AddParticipate(It.IsAny<Participate>())).Returns(response);

            //Act

            var actual = target.AddParticipate(addParticipateDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.AddParticipate(It.IsAny<Participate>()), Times.Once);

        }

        [Fact]
        public void AddParticipate_ReturnsBadRequest_WhenParticipateIsNotAdded()
        {
            var fixture = new Fixture();
            var addParticipateDto = fixture.Create<AddParticipateDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.AddParticipate(It.IsAny<Participate>())).Returns(response);

            //Act

            var actual = target.AddParticipate(addParticipateDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.AddParticipate(It.IsAny<Participate>()), Times.Once);

        }


        [Fact]
        public void NominateParticipate_ReturnsOk_WhenNominatedSuccessfully()
        {
            var fixture = new Fixture();
            var updateParticipateDto = fixture.Create<NominateParticipateDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.NominateParticipant(It.IsAny<NominateParticipateDto>())).Returns(response);

            //Act

            var actual = target.NominateParticipate(updateParticipateDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.NominateParticipant(It.IsAny<NominateParticipateDto>()), Times.Once);

        }

        [Fact]
        public void NominateParticipate_ReturnsBadRequest_WhenNotNominated()
        {
            var fixture = new Fixture();
            var updateParticipateDto = fixture.Create<NominateParticipateDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockParticipateService = new Mock<IManagerService>();
            var target = new ManagerController(mockParticipateService.Object);
            mockParticipateService.Setup(c => c.NominateParticipant(It.IsAny<NominateParticipateDto>())).Returns(response);

            //Act

            var actual = target.NominateParticipate(updateParticipateDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockParticipateService.Verify(c => c.NominateParticipant(It.IsAny<NominateParticipateDto>()), Times.Once);

        }

        [Fact]
        public void GetParticipantByManagerId_ReturnsOkWithParticipant_WhenParticipantExists()
        {
            //Arrange

            var participates = new List<ParticipateDto>
            {
                new ParticipateDto
                {
                    ParticipantId =1,
                    LastName = "test",
                    FirstName = "test",
                    Email = "S@gmail.com",
                    JobId = 1,
                    UserId = 1
                },
                 new ParticipateDto
                {
                    ParticipantId =2,
                    LastName = "test 1",
                    FirstName = "test 1",
                    Email = "S1@gmail.com",
                    JobId = 1,
                    UserId =1
                }
            };

            var response = new ServiceResponse<IEnumerable<ParticipateDto>>
            {
                Success = true,
                Data = participates // Convert to ProductDto
            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetParticipateByManageId(1)).Returns(response);

            //Act
            var actual = target.GetParticipantByManagerId(1) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetParticipateByManageId(1), Times.Once);
        }

        [Fact]
        public void GetParticipateByManagerId_ReturnsNotFound_WhenParticipateNotExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ParticipateDto>>
            {
                Success = false,

            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetParticipateByManageId(1)).Returns(response);

            //Act
            var actual = target.GetParticipantByManagerId(1) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetParticipateByManageId(1), Times.Once);
        }

        [Fact]
        public void GetParticipantById_ReturnsOkWithParticipant_WhenParticipantExists()
        {
            //Arrange

            var participates = new ParticipateDto
            { 
                    ParticipantId =1,
                    LastName = "test",
                    FirstName = "test",
                    Email = "S@gmail.com",
                    JobId = 1,
                    UserId = 1
            };

            var response = new ServiceResponse<ParticipateDto>
            {
                Success = true,
                Data = participates 
            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetParticipateById(1)).Returns(response);

            //Act
            var actual = target.GetParticipantById(1) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetParticipateById(1), Times.Once);
        }

        [Fact]
        public void GetParticipateById_ReturnsNotFound_WhenParticipateNotExists()
        {
            //Arrange
            var response = new ServiceResponse<ParticipateDto>
            {
                Success = false,

            };

            var mockManagerService = new Mock<IManagerService>();
            var target = new ManagerController(mockManagerService.Object);
            mockManagerService.Setup(c => c.GetParticipateById(1)).Returns(response);

            //Act
            var actual = target.GetParticipantById(1) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockManagerService.Verify(c => c.GetParticipateById(1), Times.Once);
        }


    }
}
