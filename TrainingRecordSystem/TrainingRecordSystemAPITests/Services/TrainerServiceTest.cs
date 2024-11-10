using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPITests.Services
{
    public class TrainerServiceTest:IDisposable
    {
        private readonly Mock<ITrainerReository> mockTrainerRepository;

        public TrainerServiceTest()
            {
            mockTrainerRepository= new Mock<ITrainerReository>();
            }
        //--------GetAllTrainingTopicbyTrainerId--------//

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllTrainingTopicbyTrainerId_ReturnsNoRecord_WhenopicNotExist()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;

            mockTrainerRepository.Setup(r => r.GetAllTrainingTopicbyTrainerId(5,page, pageSize)).Returns<IEnumerable<TrainingTopicDto>>(null);

            var contactService = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = contactService.GetAllTrainingTopicbyTrainerId(5,page, pageSize);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockTrainerRepository.Verify(r => r.GetAllTrainingTopicbyTrainerId(5, page, pageSize),Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllTrainingTopicbyTrainerId_ReturnsContacts_WhenTopicsExist()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;

            var trainingTopics = new List<TrainerTopic>
            {
               new TrainerTopic{
                   TrainerTopicId=1,
                   UserId=5,
                   TopicId=2,
                   JobId = 1,
                   User = new User
                   {
                       UserId = 5,
                       FirstName = "First name 1"
                   },
                   Job = new Job
                   {
                       JobId = 1,
                       JobName = "Developer"
                   },
                   Topic = new Topic
                   {
                       TopicId = 2,
                       TopicName = "Angular"
                   }
               },
                new TrainerTopic{
                   TrainerTopicId=2,
                   UserId=5,
                   TopicId=3,
                   JobId = 2,
                    User = new User
                   {
                       UserId = 5,
                       FirstName = "First name 1"
                   },
                   Job = new Job
                   {
                       JobId = 2,
                       JobName = "Tester"
                   },
                   Topic = new Topic
                   {
                       TopicId = 3,
                       TopicName = "Functinal Testing"
                   }
                },

             };

            mockTrainerRepository.Setup(r => r.GetAllTrainingTopicbyTrainerId(5, page, pageSize)).Returns(trainingTopics);

            var contactService = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = contactService.GetAllTrainingTopicbyTrainerId(5, page, pageSize);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(trainingTopics.Count, actual.Data.Count());
            mockTrainerRepository.Verify(r => r.GetAllTrainingTopicbyTrainerId(5, page, pageSize),Times.Once);
        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllTrainingTopicbyTrainerId_ThrowException()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;

            mockTrainerRepository.Setup(r => r.GetAllTrainingTopicbyTrainerId(5, page, pageSize)).Throws(new Exception());

            var contactService = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = contactService.GetAllTrainingTopicbyTrainerId(5, page, pageSize);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            mockTrainerRepository.Verify(r => r.GetAllTrainingTopicbyTrainerId(5, page, pageSize), Times.Once);
        }
        //-----------TotalFavouriteSpecificContacts-------//
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void TotalCountofTrainingTopicbyTrainerId_ReturnsContactCount()
        {
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

            mockTrainerRepository.Setup(r => r.TotalCountofTrainingTopicbyTrainerId(5)).Returns(trainingTopics.Count);

            var trainerService = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = trainerService.TotalCountofTrainingTopicbyTrainerId(5);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(trainingTopics.Count, actual.Data);
            mockTrainerRepository.Verify(r => r.TotalCountofTrainingTopicbyTrainerId(5), Times.Once);
        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void TotalCountofTrainingTopicbyTrainerId_WhenThrowException()
        {
            
            mockTrainerRepository.Setup(r => r.TotalCountofTrainingTopicbyTrainerId(5)).Throws(new Exception());

            var trainerService = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = trainerService.TotalCountofTrainingTopicbyTrainerId(5);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(r => r.TotalCountofTrainingTopicbyTrainerId(5), Times.Once);
        }

        //---------------GetAllParticipantsByPagination-----------
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllParticipants_ReturnErrorMessage_whenNoParticipantsExist()
        {
            //Arrange
            IEnumerable<Nomination> participants = null;
            var response = new ServiceResponse<IEnumerable<Nomination>>()
            {
                Data = participants,
                Success = false,
                Message = "No record found",
            };
            mockTrainerRepository.Setup(c => c.GetAllParticipateByPagination(1, 2, "")).Returns(participants);
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllParticipantsByPAgination(1, 2, "");

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.GetAllParticipateByPagination(1, 2, ""), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllParticipants_ReturnParticipants_whenPArticipantsExist()
        {
            //Arrange
            var participants = new List<Nomination>
            {
                new Nomination
                {
                    NominationId = 1,
                    ModePreference = "Online",
                    TopicId = 1,
                    Topic = new Topic()
                    {
                        TopicId=1,
                        TopicName = "Topic1",
                        JobId = 1,
                        Job = new Job()
                    },
                    ParticipateId = 1,
                    Participate = new Participate()
                    {
                        ParticipateId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                        Email = "test@gmail.com",
                        UserId = 1,
                        User = new User(),
                        JobId = 1,
                        Job = new Job()
                    }

                }

            };
            var response = new ServiceResponse<IEnumerable<Nomination>>()
            {
                Data = participants,
                Success = true,
                Message = "Success",
            };
            mockTrainerRepository.Setup(c => c.GetAllParticipateByPagination(1, 2, "")).Returns(participants);
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllParticipantsByPAgination(1, 2, "");

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.GetAllParticipateByPagination(1, 2, ""), Times.Once);

        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetAllParticipants_ThrowException()
        {
            //Arrange
           
            mockTrainerRepository.Setup(c => c.GetAllParticipateByPagination(1, 2, "")).Throws(new Exception());
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllParticipantsByPAgination(1, 2, "");

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(c => c.GetAllParticipateByPagination(1, 2, ""), Times.Once);

        }

        //--------------Total PArticipant Count---------------
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void TotalParticipantCount_ReturnsResponse_WhenContactExists()
        {
            //Arrange
            var participants = new List<Nomination>
            {
                new Nomination
                {
                    NominationId = 1,
                    ModePreference = "Online",
                    TopicId = 1,
                    Topic = new Topic(),
                    ParticipateId = 1,
                    Participate = new Participate()
                }

            };
       
            mockTrainerRepository.Setup(c => c.TotalNofParticipants()).Returns(1);
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.TotalParticipants();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(1, actual.Data);
            mockTrainerRepository.Verify(c => c.TotalNofParticipants(), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void TotalParticipantsCount_ReturnErrorMessage_whenNoParticipantsExist()
        {
            //Arrange
            int participants = 0;
            var response = new ServiceResponse<int>()
            {
                Data = participants,
                Success = false,
                Message = "No record found",
            };
            mockTrainerRepository.Setup(c => c.TotalNofParticipants()).Returns(participants);
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.TotalParticipants();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TotalNofParticipants(), Times.Once);

        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void TotalParticipantsCount_WhenThrowException()
        {
            //Arrange
           
            mockTrainerRepository.Setup(c => c.TotalNofParticipants()).Throws(new Exception());
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.TotalParticipants();

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(c => c.TotalNofParticipants(), Times.Once);

        }

        // ----------------- AddTrainingProgramDetail ------------------
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsSuccess_WhenDetailsAddedSuccessfully()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Training program details added successfully"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);
            mockTrainerRepository.Setup(c => c.AddTrainingProgramDetail(It.IsAny<TrainerProgramDetail>())).Returns(true);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
            mockTrainerRepository.Verify(c => c.AddTrainingProgramDetail(It.IsAny<TrainerProgramDetail>()), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenDetailsNotAdded()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };
            mockTrainerRepository.Setup(c => c.AddTrainingProgramDetail(It.IsAny<TrainerProgramDetail>())).Returns(false);

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
            mockTrainerRepository.Verify(c => c.AddTrainingProgramDetail(It.IsAny<TrainerProgramDetail>()), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenModePreferenceIsIncorrect()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Mode preference can only be either 'Hybrid', 'Online', or 'Offline'"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenTimesAreIncorrect()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start time cannot be after end time"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenDatesAreIncorrect()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-19", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start date cannot be after end date"
            };

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenStartDateIsPast()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2024-07-10", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2024-07-20", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start date needs to be a future date"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_ReturnsErrorMessage_WhenProgramDetailsAlreadyExist()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2024-07-10", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2024-07-20", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Training program details have been already added"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void AddTrainingProgramDetail_WhenThrowException()
        {
            // Arrange
            AddTrainingProgramDetailDto trainingProgramDetailDto = new AddTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2024-07-10", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2024-07-20", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "test",
                TargetAudience = "test",
            };

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Throws(new Exception());

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.AddTrainingProgramDetail(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }

        // ----------------- GetAllTraniningProgramDetails ------------------
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetTrainingProgramDetials_ReturnSuccess_whenDataExist()
        {
            //Arrange
            TrainerProgramDetail trainingProgramDetails = new TrainerProgramDetail
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Duration =1,
                ModePreference = "test",
                TargetAudience = "test",
                TrainerProgramDetailId = 1,
                TrainerTopicId = 1,
                TrainerTopic = new TrainerTopic { 
                    JobId= 1,
                    UserId = 1,
                    TrainerTopicId = 1,
                    TopicId = 1
                }
            };

            mockTrainerRepository.Setup(c => c.GetAllTrainerProgramDetails(1, 1)).Returns(trainingProgramDetails);
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllTraniningProgramDetails(1, 1);

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Data);
            mockTrainerRepository.Verify(c => c.GetAllTrainerProgramDetails(1, 2), Times.Never);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetTrainingProgramDetails_ReturnError_whenDataNotExist()
        {
            //Arrange
          
            var response = new ServiceResponse<TrainerProgramDetail>()
            {
                Data = null,
                Success = false,
                Message = "No record found!",
            };
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllTraniningProgramDetails(1, 1);

            //Assert
            Assert.NotNull(actual);
            Assert.Null(actual.Data);
            Assert.Equal(response.Message,actual.Message);
          
        }
        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void GetTrainingProgramDetails_ThrowException()
        {
            //Arrange
          
            mockTrainerRepository.Setup(c => c.GetAllTrainerProgramDetails(1, 1)).Throws(new Exception());
            var target = new TrainerService(mockTrainerRepository.Object);

            //Act
            var actual = target.GetAllTraniningProgramDetails(1, 1);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(c => c.GetAllTrainerProgramDetails(1, 2), Times.Never);

        }
        // ----------------- UpdateTrainingProgramDetails ------------------

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsSuccess_WhenDetailsUpdatedSuccessfully()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Training program details updated successfully"
            };

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);
            mockTrainerRepository.Setup(c => c.UpdateTrainingProgramDetails(It.IsAny<TrainerProgramDetail>())).Returns(true);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
            mockTrainerRepository.Verify(c => c.UpdateTrainingProgramDetails(It.IsAny<TrainerProgramDetail>()), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenDetailsNotUpdated()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);
            mockTrainerRepository.Setup(c => c.UpdateTrainingProgramDetails(It.IsAny<TrainerProgramDetail>())).Returns(false);

            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
            mockTrainerRepository.Verify(c => c.UpdateTrainingProgramDetails(It.IsAny<TrainerProgramDetail>()), Times.Once);
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenDetailsnotAdded()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Please add training program details first"
            };

            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(false);
         
            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
         
        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenStartDateIsPastDate()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2024-07-20", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start date needs to be a future date"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);


            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenStartDateIsAfterEndDate()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-20", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start date cannot be after end date"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);


            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenStarTimeisAfterEndTime()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "16:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Start time cannot be after end time"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);


            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ReturnsError_WhenModeOfPrefrenceISNotValid()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "error",
                TargetAudience = "test",
            };

            var mockResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Mode preference can only be either 'Hybrid', 'Online', or 'Offline'"
            };
            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Returns(true);


            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(mockResponse.Message, actual.Message);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);

        }

        [Fact]
        [Trait("Trainer", "TrainerServiceTests")]
        public void UpdateTrainingProgramDetails_ThrowException()
        {
            // Arrange
            UpdateTrainingProgramDetailDto trainingProgramDetailDto = new UpdateTrainingProgramDetailDto()
            {
                TrainerTopicId = 1,
                StartDate = DateTime.Parse("2025-07-16", System.Globalization.CultureInfo.CurrentCulture),
                EndDate = DateTime.Parse("2025-07-18", System.Globalization.CultureInfo.CurrentCulture),
                StartTime = "10:00:00",
                EndTime = "14:00:00",
                ModePreference = "hybrid",
                TargetAudience = "test",
            };


            mockTrainerRepository.Setup(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId)).Throws(new Exception());
           
            var target = new TrainerService(mockTrainerRepository.Object);

            // Act
            var actual = target.UpdateTrainingProgramDetails(trainingProgramDetailDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockTrainerRepository.Verify(c => c.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId), Times.Once);
        }


        public void Dispose()
        {
            mockTrainerRepository.VerifyAll();
        }

    }
}
