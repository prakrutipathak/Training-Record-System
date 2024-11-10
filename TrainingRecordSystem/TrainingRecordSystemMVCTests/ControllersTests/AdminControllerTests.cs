using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemMVC.Controllers;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVCTests.ControllersTests
{
    public class AdminControllerTests
    {
        //-------------------Index---------------
        [Fact]
        public void Index_ReturnsTrainer()
        {
            // Arrange

            var expectedContacts = new List<TrainerByPaginationViewModel>
            {
                new TrainerByPaginationViewModel
                {
                    FirstName = "FirstName1",
                    LastName ="LastName1",
                    JobId = 1,

                },
                 new TrainerByPaginationViewModel
                {
                    FirstName = "FirstName2",
                    LastName ="LastName2",
                    JobId = 2,

                }
            };

            var pageSize = 4;


            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            var countResponse = new ServiceResponse<int> { Data = expectedContacts.Count };
            var response = new ServiceResponse<IEnumerable<TrainerByPaginationViewModel>> { Success = true, Data = expectedContacts };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Index(1, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.ViewData.ContainsKey("PageSize"));
            Assert.Equal(expectedContacts, actual.Model as List<TrainerByPaginationViewModel>);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.AtLeastOnce);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


        [Fact]
        public void Index_ReturnsError_WhenNoTrainerExists()
        {
            // Arrange

            var expectedContacts = new List<TrainerByPaginationViewModel>();
            var pageSize = 6;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            var countResponse = new ServiceResponse<int> { Data = expectedContacts.Count };
            var response = new ServiceResponse<IEnumerable<TrainerByPaginationViewModel>> { Success = false };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Index(1, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.ViewData.ContainsKey("PageSize"));
            Assert.Equal(expectedContacts, actual.Model as List<TrainerByPaginationViewModel>);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.AtLeastOnce);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        //-----------------Add Traine Get---------------------------

        [Fact]
        public void Create_ReturnsView()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var countries = new List<AllJobViewModel>
            {
                new AllJobViewModel { JobId = 1,JobName="Job1"},
                new AllJobViewModel { JobId = 2,JobName="Job2"},
             };
            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = true,
                Data = countries
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ReturnsView_CountriesNotFound()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = false,
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //-----------------------Add Trainer Post--------------------
        
        [Fact]
        public void Create_TrainerSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var viewModel = new AddTrainerViewModel()
            {
                LoginId = "test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                JobId = 1,

            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Create_TrainerSavedSuccessfully_ReturnsViews()
        {
            //Arrange
            var viewModel = new AddTrainerViewModel()
            {
                LoginId = "test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                JobId = 1,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            string successMessage = null;
            var expectedServiceResponse = new ServiceResponse<AddTrainerViewModel>
            {
                Success = true,
                Data = viewModel,
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;
            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Create_TrainerFailedToSave_RedirectToAction_WithInvalidData()
        {
            //Arrange
            var viewModel = new AddTrainerViewModel
            {
                LoginId = "test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                JobId = 1,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Invalid contact number.";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Create_ReturnsSomethingWentWrong_ReturnView()
        {
            //Arrange
            var viewModel = new AddTrainerViewModel
            {
                FirstName = "C1",
                LastName = "D1"
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong please try after some time.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Create_TrainerFailed_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new AddTrainerViewModel()
            {
                FirstName = "FirstName1",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            // Mock configuration
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            // Mock HttpContext.Request
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);

            // Create a mock response for GetCountries
            var countryResponse = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Data = new List<AllJobViewModel>
                {
                   new AllJobViewModel { JobId = 1,JobName="Job1"},
                   new AllJobViewModel { JobId = 2,JobName="Job2"},
                }
            };

            // Mock the ExecuteApiRequest method
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Add model error to simulate invalid model state
            target.ModelState.AddModelError("LastName", "Last name is required.");

            // Act
            var actual = target.Create(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //--------------Assign Topic-----------------
        [Fact]
        public void AssignTopic_RedirectsToIndex_WhenUserDetailsIsNull()
        {
            // Arrange
            int userId = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(),null,60))
                .Returns<ServiceResponse<UserDetailsViewModel>>(null); // Simulate null response

            var controller = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var actual = controller.AssignTopic(userId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);

            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void AssignTopic_RedirecttoSuccess_WhenUserDetailsAvailable()
        {
            //Arrange
            var userId = 1;
            var userDetail = new ServiceResponse<UserDetailsViewModel>
            {
                Data = new UserDetailsViewModel
                {
                    UserId = userId,
                    LoginId = "loginId",
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "test@gmail.com",
                    Role = 1,
                    JobId = 1,
                }
                
            };
            var getTopicsByJobId = new ServiceResponse<IEnumerable<TopicViewModel>>
            {
                Data = new List<TopicViewModel>
                {
                    new TopicViewModel{TopicId = 1,TopicName ="topic1",JobId=1,Job = new JobViewModel(){JobId=1,JobName="Job1" } },
                    new TopicViewModel{TopicId = 2,TopicName ="topic2",JobId=2,Job = new JobViewModel(){JobId=2,JobName="Job2" }},
                }
                

            };
            var getAllAssignTopics = new ServiceResponse<IEnumerable<TrainingTopicViewModel>>
            {
                Data = new List<TrainingTopicViewModel>
                {
                    new TrainingTopicViewModel{TrainerTopicId =1,UserId=1,JobId=1,Topic=new TopicViewModel(),Job = new JobViewModel(),isTrainingScheduled=true},
                    new TrainingTopicViewModel{TrainerTopicId =2,UserId=2,JobId=2,Topic=new TopicViewModel(),Job = new JobViewModel(),isTrainingScheduled=true},
                }
                

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            // Mock configuration
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            // Mock HttpContext.Request
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(userDetail);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(getTopicsByJobId);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingTopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(getAllAssignTopics);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.AssignTopic(userId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"],Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60),Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingTopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);


        }

        [Fact]
        public void AssignTopic_RedirecttoIndex_WhenUserDetailsNotNull()
        {
            // Arrange
            int userId = 1;
            var userDetail = new ServiceResponse<UserDetailsViewModel>
            {
                Data = new UserDetailsViewModel
                {
                    UserId = userId,
                    LoginId = "loginId",
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "test@gmail.com",
                    Role = 1,
                    JobId = 1,
                },
                Success=false

            };
            var getTopicsByJobId = new ServiceResponse<IEnumerable<TopicViewModel>>
            {
                Data = new List<TopicViewModel>
                {
                    new TopicViewModel{TopicId = 1,TopicName ="topic1",JobId=1,Job = new JobViewModel(){JobId=1,JobName="Job1" } },
                    new TopicViewModel{TopicId = 2,TopicName ="topic2",JobId=2,Job = new JobViewModel(){JobId=2,JobName="Job2" }},
                },
                Success = false


            };
            var getAllAssignTopics = new ServiceResponse<IEnumerable<TrainingTopicViewModel>>
            {
                Data = new List<TrainingTopicViewModel>
                {
                    new TrainingTopicViewModel{TrainerTopicId =1,UserId=1,JobId=1,Topic=new TopicViewModel(),Job = new JobViewModel(),isTrainingScheduled=true},
                    new TrainingTopicViewModel{TrainerTopicId =2,UserId=2,JobId=2,Topic=new TopicViewModel(),Job = new JobViewModel(),isTrainingScheduled=true},
                },
                Success = false


            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(userDetail);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(getTopicsByJobId);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingTopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(getAllAssignTopics);

            var controller = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var actual = controller.AssignTopic(userId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingTopicViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        //------------Monthly Admin Report-----------
        [Fact]
        public void MonthAdminReport_ReturnSuccess_WhenUserIDFound()
        {
            //Arrange
            int userId = 1;
            var report = new ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>
            {
                Data = new List<MonthlyAdminReportViewModel>
                {
                    new MonthlyAdminReportViewModel{TopicName="Topic1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new MonthlyAdminReportViewModel{TopicName="Topic2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                }
            };

            var trainers = new ServiceResponse<IEnumerable<AllTrainersViewModel>>
            {
                Data = new List<AllTrainersViewModel>
                {
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName1",LastName="LastName1",JobId=1,Job = new AllJobViewModel()},
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName2",LastName="LastName2",JobId=2,Job = new AllJobViewModel()},


                }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(trainers);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.MonthlyAdminReport(userId,null,null);

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60),Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void MonthAdminReport_ReturnSuccess_WhenUserIDMonthYearFound()
        {
            //Arrange
            int userId = 1;
            var report = new ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>
            {
                Data = new List<MonthlyAdminReportViewModel>
                {
                    new MonthlyAdminReportViewModel{TopicName="Topic1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new MonthlyAdminReportViewModel{TopicName="Topic2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                }
            };

            var trainers = new ServiceResponse<IEnumerable<AllTrainersViewModel>>
            {
                Data = new List<AllTrainersViewModel>
                {
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName1",LastName="LastName1",JobId=1,Job = new AllJobViewModel()},
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName2",LastName="LastName2",JobId=2,Job = new AllJobViewModel()},


                }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(trainers);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.MonthlyAdminReport(userId, 1, 1);

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void MonthAdminReport_ReturnList_WhenSuccessFalse()
        {
            //Arrange
            int userId = 1;
            var report = new ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>
            {
                Data = new List<MonthlyAdminReportViewModel>
                {
                    new MonthlyAdminReportViewModel{TopicName="Topic1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new MonthlyAdminReportViewModel{TopicName="Topic2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                },
                Success=false
                
            };

            var trainers = new ServiceResponse<IEnumerable<AllTrainersViewModel>>
            {
                Data = new List<AllTrainersViewModel>
                {
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName1",LastName="LastName1",JobId=1,Job = new AllJobViewModel()},
                    new AllTrainersViewModel{UserId=1,FirstName="FirstName2",LastName="LastName2",JobId=2,Job = new AllJobViewModel()},


                },
                Success = false
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(trainers);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.MonthlyAdminReport(userId, 1, 1);

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        //---------------Date Range Report -------------------------
        [Fact]
        public void DaterangeBasedReport_ReturnSuccess_WhenJobIdFound()
        {
            //Arrange
            int JobId = 1;
            var report = new ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>
            {
                Data = new List<DaterangeBasedReportViewModel>
                {
                    new DaterangeBasedReportViewModel{TopicName="Topic1", TrainerName="Trainer1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new DaterangeBasedReportViewModel{TopicName="Topic2", TrainerName="Trainer2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                }
            };

            var jobs = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Data = new List<AllJobViewModel>
                {
                    new AllJobViewModel{JobId=1,JobName="job1"},
                    new AllJobViewModel{JobId=2,JobName="job2"},

                }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(jobs);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.DaterangeBasedReport(JobId, null, null);

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void DaterangeBasedReport_ReturnSuccess_WhenJobIdStartDateEndDateFound()
        {
            //Arrange
            int JobId = 1;
            var report = new ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>
            {
                Data = new List<DaterangeBasedReportViewModel>
                {
                    new DaterangeBasedReportViewModel{TopicName="Topic1", TrainerName="Trainer1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new DaterangeBasedReportViewModel{TopicName="Topic2", TrainerName="Trainer2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                }
            };

            var jobs = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Data = new List<AllJobViewModel>
                {
                    new AllJobViewModel{JobId=1,JobName="job1"},
                    new AllJobViewModel{JobId=2,JobName="job2"},

                }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(jobs);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.DaterangeBasedReport(JobId, DateTime.Now, DateTime.Now.AddDays(2));

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void DaterangeBasedReport_ReturnList_WhenSuccessFalse()
        {
            //Arrange
            int JobId = 1;
            var report = new ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>
            {
                Data = new List<DaterangeBasedReportViewModel>
                {
                    new DaterangeBasedReportViewModel{TopicName="Topic1", TrainerName="Trainer1",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=1,ModePreference="Online",TotalParticipateNo=10},
                    new DaterangeBasedReportViewModel{TopicName="Topic2", TrainerName="Trainer2",StartDate=DateTime.Now,EndDate=DateTime.Now.AddDays(2),Duration=2,ModePreference="Online",TotalParticipateNo=5},

                },
                Success =false

            };

            var jobs = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Data = new List<AllJobViewModel>
                {
                    new AllJobViewModel{JobId=1,JobName="job1"},
                    new AllJobViewModel{JobId=2,JobName="job2"},

                }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(report);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(jobs);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            //Act
            var actual = target.DaterangeBasedReport(JobId, DateTime.Now, DateTime.Now.AddDays(2));

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
    }
}
