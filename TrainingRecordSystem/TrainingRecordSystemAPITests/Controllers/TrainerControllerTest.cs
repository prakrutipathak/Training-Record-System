using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Controllers;
using TrainingRecordSystemAPI.Data;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPITests.Controllers
{
    public class TrainerControllerTest
    {

        //-----------GetAllTrainingTopicbyTrainerId----------//

        [Fact]
        public void GetAllTrainingTopicbyTrainerId_ReturnsOkWithTopics()
        {
            //Arrange
            var trainingTopics = new List<TrainingTopicDto>
            {
               new TrainingTopicDto{
                   TrainerTopicId=1,
                   UserId=5,
                   TopicId=2,
                   JobId = 1},
                new TrainingTopicDto{
                   TrainerTopicId=2,
                   UserId=5,
                   TopicId=3,
                   JobId = 2},

             };

            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<TrainingTopicDto>>
            {
                Success = true,
                Data = trainingTopics
            };

            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllTrainingTopicbyTrainerId(5, page, pageSize)).Returns(response);

            //Act
            var actual = target.GetAllTrainingTopicbyTrainerId(5, page, pageSize) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllTrainingTopicbyTrainerId(5, page, pageSize), Times.Once);
        }

        [Fact]
        public void GetAllTrainingTopicbyTrainerId_ReturnsNotFound()
        {
            //Arrange
            var trainingTopics = new List<TrainingTopicDto> { };


            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<TrainingTopicDto>>
            {
                Success = false,
                Data = trainingTopics
            };


            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllTrainingTopicbyTrainerId(5, page, pageSize)).Returns(response);

            //Act
            var actual = target.GetAllTrainingTopicbyTrainerId(5, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllTrainingTopicbyTrainerId(5, page, pageSize), Times.Once);
        }

        //---------TotalCountofTrainingTopicbyTrainerId---------//
        [Fact]
        public void TotalCountofTrainingTopicbyTrainerId_ReturnsOkWithCount()
        {
            //Arrange
            var trainingTopics = new List<TrainingTopicDto>
            {
               new TrainingTopicDto{
                   TrainerTopicId=1,
                   UserId=5,
                   TopicId=2,
                   JobId = 1},
                new TrainingTopicDto{
                   TrainerTopicId=2,
                   UserId=5,
                   TopicId=3,
                   JobId = 2},

             };

            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = trainingTopics.Count
            };

            var mockCountService = new Mock<ITrainerService>();
            var target = new TrainerController(mockCountService.Object);
            mockCountService.Setup(c => c.TotalCountofTrainingTopicbyTrainerId(5)).Returns(response);

            //Act
            var actual = target.TotalCountofTrainingTopicbyTrainerId(5) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockCountService.Verify(c => c.TotalCountofTrainingTopicbyTrainerId(5), Times.Once);
        }


        [Fact]
        public void TotalCountofTrainingTopicbyTrainerId_ReturnsNotFound()
        {
            //Arrange
            var trainingTopics = new List<TrainingTopicDto> { };

            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = trainingTopics.Count
            };

            var mockCountService = new Mock<ITrainerService>();
            var target = new TrainerController(mockCountService.Object);
            mockCountService.Setup(c => c.TotalCountofTrainingTopicbyTrainerId(5)).Returns(response);

            //Act
            var actual = target.TotalCountofTrainingTopicbyTrainerId(5) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockCountService.Verify(c => c.TotalCountofTrainingTopicbyTrainerId(5), Times.Once);
        }

        //---------Get All Participants ------------
        [Fact]
        public void GetAllParticipants_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string sort_dir = "default";

            var participants = new List<NominationDto>()
            {
                new NominationDto()
                {
                    NominationId = 1,
                    ModePreference = "Online",
                    TopicId = 1,
                    Topic = new Topic(),
                    ParticipateId = 1,
                    Participate = new Participate()

                },
                new NominationDto()
                {
                    NominationId = 2,
                    ModePreference = "Online",
                    TopicId = 2,
                    Topic = new Topic(),
                    ParticipateId = 2,
                    Participate = new Participate()

                },
            };

            var response = new ServiceResponse<IEnumerable<NominationDto>>()
            {
                Data = participants,
                Success = true,
                Message = "",
            };

            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.GetAllParticipantsByPAgination(page, page_size, sort_dir)).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.GetAllTraineryPagination(page, page_size, sort_dir) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.GetAllParticipantsByPAgination(page, page_size, sort_dir), Times.Once);
        }

        [Fact]
        public void GetAllParticipants_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string sort_dir = "default";

            var response = new ServiceResponse<IEnumerable<NominationDto>>()
            {
                Success = false,
                Message = "",
            };

            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.GetAllParticipantsByPAgination(page, page_size, sort_dir)).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.GetAllTraineryPagination(page, page_size, sort_dir) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.GetAllParticipantsByPAgination(page, page_size, sort_dir), Times.Once);
        }

        //-----------------Get Total Contacts----------------
        [Fact]
        public void GetTotalParticipants_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 10,
                Success = true,
                Message = "",
            };

            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.TotalParticipants()).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.GetTotalCountOfPositions() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.TotalParticipants(), Times.Once);
        }

        [Fact]
        public void GetTotalParticipants_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 0,
                Success = false,
                Message = "",
            };

            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.TotalParticipants()).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.GetTotalCountOfPositions() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.TotalParticipants(), Times.Once);
        }

        // -------------- AddTrainingProgramDetail
        [Fact]
        public void AddTrainingProgramDetail_ReturnsOkResponse_WhenAssignedSuccessfully()
        {
            // Arrange
            var detailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
            };
            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Updated successfully"
            };
            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.AddTrainingProgramDetail(detailDto)).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(detailDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.AddTrainingProgramDetail(detailDto), Times.Once);
        }

        [Fact]
        public void AddTrainingProgramDetail_ReturnsBadRequestResponse_WhenNotAssignedSuccessfully()
        {
            // Arrange
            var detailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Updated successfully"
            };
            var mockTrainerService = new Mock<ITrainerService>();
            mockTrainerService.Setup(c => c.AddTrainingProgramDetail(detailDto)).Returns(response);

            var target = new TrainerController(mockTrainerService.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(detailDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTrainerService.Verify(c => c.AddTrainingProgramDetail(detailDto), Times.Once);
        }

        // -------------- GetAllTrainingProgrambyTrainerId

        [Fact]
        public void GetAllTrainingProgrambyTrainerId_ReturnsOkWithdetials()
        {
            //Arrange
            var trainingTopics = new TrainingProgramDetailsDto
            {
                TrainerProgramDetailId = 1,
                StartDate = new DateTime(),
                EndDate = new DateTime(),
                StartTime = new DateTime(),
                EndTime = new DateTime(),
                Duration = 1,
                ModePreference = "test",
               TargetAudience = "test",
               TrainerTopicId = 1
             };

            var response = new ServiceResponse<TrainingProgramDetailsDto>
            {
                Success = true,
                Data = trainingTopics
            };

            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllTraniningProgramDetails(1,2)).Returns(response);

            //Act
            var actual = target.GetAllTrainingProgrambyTrainerId(1,2) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllTraniningProgramDetails(1,2), Times.Once);
        }

        [Fact]
        public void GetAllTrainingProgrambyTrainerId_ReturnsNotFound()
        {
            //Arrange
            var trainingTopics = new TrainingProgramDetailsDto();

            var response = new ServiceResponse<TrainingProgramDetailsDto>
            {
                Success = false,
                Data = trainingTopics
            };


            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllTraniningProgramDetails(1, 2)).Returns(response);

            //Act
            var actual = target.GetAllTrainingProgrambyTrainerId(1, 2) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllTraniningProgramDetails(1, 2), Times.Once);
        }
        // -------------- UpdateTrainingProgramDetail
        [Fact]
        public void UpdateTrainingProgramDetail_ReturnsOkWithdetials()
        {
            //Arrange
            var trainingTopics = new UpdateTrainingProgramDetailDto
            {
                TrainerProgramDetailId = 1,
                StartDate = new DateTime(),
                EndDate = new DateTime(),
                StartTime = "12",
                EndTime = "12",
                ModePreference = "test",
                TargetAudience = "test",
                TrainerTopicId = 1
            };

            var response = new ServiceResponse<string>
            {
                Success = true,
                Data = null,
                Message = "Success"
            };

            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.UpdateTrainingProgramDetails(trainingTopics)).Returns(response);

            //Act
            var actual = target.UpdateTrainingProgramDetail(trainingTopics) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateTrainingProgramDetails(trainingTopics), Times.Once);
        }

        [Fact]
        public void UpdateTrainingProgramDetail_ReturnsBadRequest()
        {
            //Arrange
            var trainingTopics = new UpdateTrainingProgramDetailDto();

            var response = new ServiceResponse<string>
            {
                Success = false,
                Data = null,
                Message = "Something went wrong"
            };


            var mockContactService = new Mock<ITrainerService>();
            var target = new TrainerController(mockContactService.Object);
            mockContactService.Setup(c => c.UpdateTrainingProgramDetails(trainingTopics)).Returns(response);

            //Act
            var actual = target.UpdateTrainingProgramDetail(trainingTopics) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateTrainingProgramDetails(trainingTopics), Times.Once);
        }

    }
}
