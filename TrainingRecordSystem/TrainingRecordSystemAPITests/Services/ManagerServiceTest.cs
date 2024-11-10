using Fare;
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
using TrainingRecordSystemAPI.Services.Contract;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPITests.Services
{
    public class ManagerServiceTest : IDisposable
    {
        private readonly Mock<IManagerRepository> mockRepository;
        public ManagerServiceTest()
        {
            mockRepository=new Mock<IManagerRepository>();
         }

        //UpcomingTrainingProgram
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void UpcomingTrainingProgram_ReturnList_WhenNoDataExist()
        {
            //Arrange
            
            var target = new ManagerService(mockRepository.Object);
            //Act
            var actual = target.UpcomingTrainingProgram(null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void UpcomingTrainingProgram_ReturnsContactsList_WhenDataExist()
        {
            int jobId = 1;
            //Arrange
            var reports = new List<ManagerReport>
        {
            new ManagerReport
            {
               JobName = "Developer",
                TrainerName = "Trainer 1",
                TopicName = "Web Development",


            },
            new ManagerReport
            {
                JobName = "Developer",
                TrainerName = "Trainer 2",
                TopicName = "C#",
               
                
            }
        };
 
            mockRepository.Setup(c => c.UpcomingTrainingProgram(jobId)).Returns(reports);
            var target = new ManagerService(mockRepository.Object);

            //Act
            var actual = target.UpcomingTrainingProgram(jobId);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockRepository.Verify(c => c.UpcomingTrainingProgram(jobId), Times.Once);
            Assert.Equal(reports.Count, actual.Data.Count()); // Ensure the counts are equal

            for (int i = 0; i < reports.Count; i++)
            {
                Assert.Equal(reports[i].TrainerName, actual.Data.ElementAt(i).TrainerName);
                Assert.Equal(reports[i].TopicName, actual.Data.ElementAt(i).TopicName);
                Assert.Equal(reports[i].JobName, actual.Data.ElementAt(i).JobName);
            }
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void UpcomingTrainingProgram_WhenThrowException()
        {
            int jobId = 1;
            //Arrange
            mockRepository.Setup(c => c.UpcomingTrainingProgram(jobId)).Throws(new Exception()); 
            var target = new ManagerService(mockRepository.Object);

            //Act
            var actual = target.UpcomingTrainingProgram(jobId);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockRepository.Verify(c => c.UpcomingTrainingProgram(jobId), Times.Once);
          
        }
        //AddParticipate
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void AddParticipate_ReturnsSomethingWentWrong_WhenParticipantisNotSaved()
        {
            var participate = new Participate()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                JobId = 1,
                UserId=2
            };

            mockRepository.Setup(r => r.ParticipateExists(participate.Email)).Returns(false);
            mockRepository.Setup(r => r.InsertParticipate(participate)).Returns(false);
            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.AddParticipate(participate);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong try after sometime", actual.Message);
            mockRepository.Verify(r => r.ParticipateExists(participate.Email), Times.Once);
            mockRepository.Verify(r => r.InsertParticipate(participate), Times.Once);


        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void AddParticipate_ParticipateAlreadyExists_WhenParticipantisNotSaved()
        {
            var participate = new Participate()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                JobId = 1,
                UserId = 2
            };

            mockRepository.Setup(r => r.ParticipateExists(participate.Email)).Returns(true);
            
            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.AddParticipate(participate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Participate already exists.", actual.Message);
            mockRepository.Verify(r => r.ParticipateExists(participate.Email), Times.Once);
            
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void AddParticipate_WhenParticipateSavedSuccessfully()
        {
            var participate = new Participate()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                JobId = 1,
                UserId = 2
            };
            mockRepository.Setup(r => r.ParticipateExists(participate.Email)).Returns(false);
            mockRepository.Setup(r => r.InsertParticipate(participate)).Returns(true);

            var contactService = new ManagerService(mockRepository.Object);

            // Act
            var actual = contactService.AddParticipate(participate);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Participate saved successfully", actual.Message);
            mockRepository.Verify(r => r.ParticipateExists(participate.Email), Times.Once);
            mockRepository.Verify(r => r.InsertParticipate(participate), Times.Once);


        }
        
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void AddParticipate_ThrowsException()
        {
            var participate = new Participate()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                JobId = 1,
                UserId = 2
            };

            mockRepository.Setup(r => r.ParticipateExists(participate.Email)).Throws(new Exception());
            var contactService = new ManagerService(mockRepository.Object);

            // Act
            var actual = contactService.AddParticipate(participate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockRepository.Verify(r => r.ParticipateExists(participate.Email), Times.Once);
          
        }
       
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_ReturnAlreadyNominated_WhenParticipantAlreadyNominated()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Online",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,
                UserId = 2
            };
            mockRepository.Setup(r => r.NominationExists(nominate.TopicId,nominate.TrainerId,nominate.ParticipateId)).Returns(true);

            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.NominateParticipant(nominate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Participate is nominated already.", actual.Message);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId),Times.Once);

        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_Return5participantAlready_When5ParticipantAlreadyExists()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Online",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,
                UserId = 2
            };

            mockRepository.Setup(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId)).Returns(false);

            mockRepository.Setup(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId)).Returns(6);
            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.NominateParticipant(nominate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("5 Participant nominated already.", actual.Message);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId), Times.Once);
            mockRepository.Verify(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId), Times.Once);

        }

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_ReturnSpecificModePreference_WhenModePreferenceIsHybrid()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Hybrid",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,
                UserId = 2
            };
            mockRepository.Setup(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId)).Returns(false);

            mockRepository.Setup(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId)).Returns(4);

            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.NominateParticipant(nominate);
             // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Please specify either 'Offline' or 'Online' for Hybrid mode.", actual.Message);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId), Times.Once);
            mockRepository.Verify(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId), Times.Once);

        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_ReturnsSomethingWentWrong_WhenNominatedNotSuccessfully()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Online",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,
                UserId = 2
            };

            var nomination = new Nomination()
            {

                ModePreference = nominate.ModePreference,
                TopicId = nominate.TopicId,
                TrainerId = nominate.TrainerId,
                ParticipateId = nominate.TrainerId,
            };
            
            mockRepository.Setup(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId)).Returns(false);
            mockRepository.Setup(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId)).Returns(4);
            var managerService = new ManagerService(mockRepository.Object);

            // Act
           var actual = managerService.NominateParticipant(nominate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId), Times.Once);
            mockRepository.Verify(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId), Times.Once);

        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_ReturnsNominateSuccessfully_WhenNominateSuccessfully()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Online",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,
               
            };

            
            mockRepository.Setup(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId)).Returns(false);
            mockRepository.Setup(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId)).Returns(4);
            mockRepository.Setup(r => r.AddNomination(It.IsAny<Nomination>())).Returns(true);
            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.NominateParticipant(nominate);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Nominated successfully", actual.Message);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId), Times.Once);
            mockRepository.Verify(r => r.ManagerCountForParticipate(nominate.UserId, nominate.TopicId, nominate.TrainerId), Times.Once);

        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void NominateParticipant_WhenThrowException()
        {
            var nominate = new NominateParticipateDto()
            {
                ModePreference = "Online",
                TopicId = 1,
                TrainerId = 1,
                ParticipateId = 1,

            };

            mockRepository.Setup(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId)).Throws(new Exception());
            var managerService = new ManagerService(mockRepository.Object);

            // Act
            var actual = managerService.NominateParticipant(nominate);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockRepository.Verify(r => r.NominationExists(nominate.TopicId, nominate.TrainerId, nominate.ParticipateId), Times.Once);
         
        }

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipantByManagerId_ReturnsParticipantById_WhenParticipantExists()
        {
            var participatesDto = new List<ParticipateDto>
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
            var participates = new List<Participate>
            {
                new Participate
                {
                    ParticipateId =1,
                    LastName = "test",
                    FirstName = "test",
                    Email = "S@gmail.com",
                   
                    JobId = 1,
                    UserId = 1,
                    Job = new Job
                    {
                        JobId = 1,
                        JobName = "test"
                    }
                },
                 new Participate
                {
                    ParticipateId =2,
                    LastName = "test 1",
                    FirstName = "test 1",
                 
                    Email = "S1@gmail.com",
                   
                    JobId = 1,
                    UserId =1,
                    Job = new Job
                    {
                        JobId = 1,
                        JobName = "test"
                    }
                }
            };
           
            mockRepository.Setup(c => c.GetParticipatesByManagerId(1)).Returns(participates);
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateByManageId(1);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(participatesDto.ToString(), actual.Data.ToString());
            mockRepository.Verify(c => c.GetParticipatesByManagerId(1), Times.Once);
        }

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipantByManagerId_ReturnsErrorMeesssag_WhenParticipantisNull()
        {
           
            var response = new ServiceResponse<IEnumerable<ParticipateDto>>
            {
                Success = false,
                Message = "No record found!"
            };
           
            mockRepository.Setup(c => c.GetParticipatesByManagerId(1)).Returns<ParticipateDto>(null);
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateByManageId(1);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockRepository.Verify(c => c.GetParticipatesByManagerId(1), Times.Once);
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipantByManagerId_WhenThrowException()
        {
            mockRepository.Setup(c => c.GetParticipatesByManagerId(1)).Throws(new Exception());
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateByManageId(1);

            // Assert
            Assert.NotNull(actual);
            mockRepository.Verify(c => c.GetParticipatesByManagerId(1), Times.Once);
        }
        //------------------Get Participant by id----------------
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipant_ReturnsParticipantById_WhenParticipantExists()
        {
            var participates = new Participate
            {
                ParticipateId = 1,
                LastName = "test",
                FirstName = "test",
                Email = "S@gmail.com",
             
                JobId = 1,
                UserId = 1
            };
            var ParticipantDto = new ParticipateDto
            {
                    ParticipantId =1,
                    LastName = "test",
                    FirstName = "test",
                  
                    Email = "S@gmail.com",
                
                    JobId = 1,
                    UserId = 1
            };
            mockRepository.Setup(c => c.GetParticipate(1)).Returns(participates);
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateById(1);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(ParticipantDto.ToString(), actual.Data.ToString());
            mockRepository.Verify(c => c.GetParticipate(1), Times.Once);
        }

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipant_ReturnsErrorMeesssag_WhenParticipantisNull()
        {
            var response = new ServiceResponse<ParticipateDto>
            {
                Success = false,
                Message = "No record found!"
            };
          
            mockRepository.Setup(c => c.GetParticipate(1)).Returns<ParticipateDto>(null);
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateById(1);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockRepository.Verify(c => c.GetParticipate(1), Times.Once);
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetParticipant_WhenThrowException()
        {
            mockRepository.Setup(c => c.GetParticipate(1)).Throws(new Exception());
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetParticipateById(1);

            // Assert
            Assert.NotNull(actual);
            mockRepository.Verify(c => c.GetParticipate(1), Times.Once);
        }

        //------GetModeofTrainingByTopicId-------

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetModeofTrainingByTopicId_ReturnsModeOfPrefrence_WhenModeOfPrefrenceExists()
        {
       
            int topicId = 1;
            int userId = 1;
            string modeOfTraining = "online";

            mockRepository.Setup(c => c.GetModeofTrainingByTopicId(userId,topicId)).Returns(modeOfTraining);
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetModeofTrainingByTopicId(userId, topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(modeOfTraining, actual.Data.ToString());
            mockRepository.Verify(c => c.GetModeofTrainingByTopicId(userId,topicId));
        }

        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetModeofTrainingByTopicId_ReturnsErrorMeesssag_WhenModeOfPrefrenceisNull()
        {
            int topicId = 1;

            int userId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "No record found!"
            };

            mockRepository.Setup(c => c.GetModeofTrainingByTopicId(userId, topicId)).Returns("");
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetModeofTrainingByTopicId(userId,topicId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockRepository.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId));
        }
        [Fact]
        [Trait("Manager", "ManagerServiceTests")]
        public void GetModeofTrainingByTopicId_WhenThrowException()
        {
            int topicId = 1;
            int userId = 1;
          
            mockRepository.Setup(c => c.GetModeofTrainingByTopicId(userId, topicId)).Throws(new Exception()); ;
            var target = new ManagerService(mockRepository.Object);

            // Act 
            var actual = target.GetModeofTrainingByTopicId(userId, topicId);

            // Assert
            Assert.NotNull(actual);
            mockRepository.Verify(c => c.GetModeofTrainingByTopicId(userId, topicId));
        }
        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        }
}
