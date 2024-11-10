using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Controllers;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPITests.Controllers
{
    public class TopicControllerTest
    {
        // --------------- GetTopicsByJobId ----------------
        [Fact]
        public void GetTopicsByJobId_ReturnsOkResponse_WhenAssignedSuccessfully()
        {
            // Arrange
            int jobId = 1;
            var response = new ServiceResponse<IEnumerable<TopicDto>>()
            {
                Success = true,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTopicsByJobId(jobId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }

        [Fact]
        public void GetTopicsByJobId_ReturnsNotFoundResponse_WhenNotAssignedSuccessfully()
        {
            // Arrange
            int jobId = 1;
            var response = new ServiceResponse<IEnumerable<TopicDto>>()
            {
                Success = false,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTopicsByJobId(jobId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTopicsByJobId(jobId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTopicsByJobId(jobId), Times.Once);
        }
        //-----------GET TRAINER TOPIC BY JOBID-----------
        [Fact]
        public void GetTrainerTopicsByJobId_ReturnsOkResponse_WhenAssignedSuccessfully()
        {
            // Arrange
            int jobId = 1;
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = true,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }

        [Fact]
        public void GetTrainerTopicsByJobId_ReturnsNotFoundResponse_WhenNotAssignedSuccessfully()
        {
            // Arrange
            int jobId = 1;
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = false,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTrainerTopicsByJobId(jobId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTrainerTopicsByJobId(jobId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTrainerTopicsByJobId(jobId), Times.Once);
        }
        //-----------GET TRAINER BY TOPIC ID-----------
        [Fact]
        public void GetTrainerByTopicId_ReturnsOkResponse()
        {
            // Arrange
            int topicId = 1;
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = true,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTrainerByTopicId(topicId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTrainerByTopicId(topicId), Times.Once);
        }

        [Fact]
        public void GetTrainerByTopicId_ReturnsNotFoundResponse()
        {
            // Arrange
            int topicId = 1;
            var response = new ServiceResponse<IEnumerable<TrainingProgramDetailJob>>()
            {
                Success = false,
                Message = "Success"
            };
            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(c => c.GetTrainerByTopicId(topicId)).Returns(response);

            var target = new TopicController(mockTopicService.Object);

            // Act
            var actual = target.GetTrainerByTopicId(topicId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTopicService.Verify(c => c.GetTrainerByTopicId(topicId), Times.Once);
        }

    }
}
