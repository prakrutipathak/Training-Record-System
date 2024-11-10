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
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemMVC.Controllers;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVCTests.ControllersTests
{
    public class ManagerControllerTests
    {
        //-----------------Add Participants Get---------------------------

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

            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.AddParticipant() as ViewResult;

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
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.AddParticipant() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //-----------------------Add Participants Post--------------------

        [Fact]
        public void Create_ParticipantSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var viewModel = new AddParticipateViewModel()
            {
                ParticipateId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                UserId = 1,
                User = new UserViewModel(),
                JobId = 1,
                Job = new JobViewModel()

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
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddParticipant(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("GetParticipantsByManagerId", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Create_ParticipantSavedSuccessfully_ReturnsViews()
        {
            //Arrange
            var viewModel = new AddParticipateViewModel()
            {
                ParticipateId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                UserId = 1,
                User = new UserViewModel(),
                JobId = 1,
                Job = new JobViewModel()

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            string successMessage = null;
            var expectedServiceResponse = new ServiceResponse<AddParticipateViewModel>
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
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },
            };

            //Act
            var actual = target.AddParticipant(viewModel) as ViewResult;
            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Create_ParticipantFailedToSave_RedirectToAction_WithInvalidData()
        {
            //Arrange
            var viewModel = new AddParticipateViewModel()
            {
                ParticipateId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "test1@gmail.com",
                UserId = 1,
                User = new UserViewModel(),
                JobId = 1,
                Job = new JobViewModel()

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

            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddParticipant(viewModel) as ViewResult;

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
            var viewModel = new AddParticipateViewModel
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
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddParticipant(viewModel) as ViewResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Create_ParticipantFailed_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new AddParticipateViewModel()
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

            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Add model error to simulate invalid model state
            target.ModelState.AddModelError("LastName", "Last name is required.");

            // Act
            var actual = target.AddParticipant(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //----------------UpcomimngTrainingProgram-----------------
        [Fact]
        public void UpcomingTrainingProgram_WhenJobIdIsZero_ReturnViewResult()
        {
            //Arrange
            var jobId = 0;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var jobs = new List<AllJobViewModel>
            {
                new AllJobViewModel { JobId = 1,JobName="Job1"},
                new AllJobViewModel { JobId = 2,JobName="Job2"},
             };
            var report = new List<UpcomingTrainingViewModel>
            {
                new UpcomingTrainingViewModel {JobName="Job1",TopicName="Topic 1",TrainerName="Trainer 1"},
                new UpcomingTrainingViewModel {JobName="Job2",TopicName="Topic 2",TrainerName="Trainer 2"},
             };
            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = true,
                Data = jobs
            };
            var expectedResponseReport = new ServiceResponse<IEnumerable<UpcomingTrainingViewModel>>
            {
                Success = true,
                Data = report
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponseReport))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(),It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);


            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.UpcomimngTrainingProgram(jobId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()),Times.Once);

        }
        [Fact]
        public void UpcomingTrainingProgram_WhenJobIdIsNotZero_ReturnRedirectToActionWhenServiceIsNull()
        {
            //Arrange
            var jobId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var jobs = new List<AllJobViewModel>
            {
                new AllJobViewModel { JobId = 1,JobName="Job1"},
                new AllJobViewModel { JobId = 2,JobName="Job2"},
             };
           
            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = true,
                Data = jobs,
        
            };
            var message = "ErrorMessage";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.UpcomimngTrainingProgram(jobId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index",actual.ActionName);
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.Equal(message, target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void UpcomingTrainingProgram_WhenJobIdIsNotZero_ReturnRedirectToActionWhenErrorResponseIsNull()
        {
            //Arrange
            var jobId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var jobs = new List<AllJobViewModel>
            {
                new AllJobViewModel { JobId = 1,JobName="Job1"},
                new AllJobViewModel { JobId = 2,JobName="Job2"},
             };
           
            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = true,
                Data = jobs,

            };
            var message = "Something went wrong please try after some time.";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.UpcomimngTrainingProgram(jobId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", actual.ActionName);
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.Equal(message, target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void UpcomingTrainingProgram_WhenJobIdIsNotZero_ReturnRedirectToActionWhenErrorResponseIsNotNull()
        {
            //Arrange
            var jobId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var jobs = new List<AllJobViewModel>
            {
                new AllJobViewModel { JobId = 1,JobName="Job1"},
                new AllJobViewModel { JobId = 2,JobName="Job2"},
             };
            var report = new List<UpcomingTrainingViewModel>
            {
                new UpcomingTrainingViewModel {JobName="Job1",TopicName="Topic 1",TrainerName="Trainer 1"},
                new UpcomingTrainingViewModel {JobName="Job2",TopicName="Topic 2",TrainerName="Trainer 2"},
             };
            var expectedResponseCountries = new ServiceResponse<IEnumerable<AllJobViewModel>>
            {
                Success = true,
                Data = jobs,

            };
            var expectedResponseReport = new ServiceResponse<IEnumerable<UpcomingTrainingViewModel>>
            {
                Success = true,
                Data = report,
                Message="Error Message"
            };
           
           
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponseReport))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.UpcomimngTrainingProgram(jobId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", actual.ActionName);
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.Equal(expectedResponseReport.Message, target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        //--------------------------GetParticipantsByManagerId-----------------------
        [Fact]
        public void GetParticipantsByManagerId_ReturnViewResult()
        {
            //Arrange
           
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
           
            var report = new List<ParticipateViewModel>
            {
                new ParticipateViewModel {FirstName="FirstName",LastName="LastName"},
                new ParticipateViewModel {FirstName="FirstName",LastName="LastName"},
             };
             var claims = new List<Claim>
            {
                new Claim("UserId", "1")
            };
            var identity = new GenericIdentity("username");
            identity.AddClaims(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsPrincipal),
            };

            var expectedResponseReport = new ServiceResponse<IEnumerable<ParticipateViewModel>>
            {
                Success = true,
                Data = report
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponseReport))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
          
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetParticipantsByManagerId() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void GetParticipantsByManagerId_RedirectToActionResult_ServiceResponseIsNull()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var message = "Something went wrong please try after some time.";
            var claims = new List<Claim>
            {
                new Claim("UserId", "1")
            };
            var identity = new GenericIdentity("username");
            identity.AddClaims(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsPrincipal),
            };

           
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetParticipantsByManagerId() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void GetParticipantsByManagerId_RedirectToActionResult_ErrorResponseIsNull()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var message = "Something went wrong please try after some time.";
            var claims = new List<Claim>
            {
                new Claim("UserId", "1")
            };
            var identity = new GenericIdentity("username");
            identity.AddClaims(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsPrincipal),
            };


            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetParticipantsByManagerId() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void GetParticipantsByManagerId_RedirectToActionResult_ErrorResponseIsNotNull()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            
            var claims = new List<Claim>
            {
                new Claim("UserId", "1")
            };
            var identity = new GenericIdentity("username");
            identity.AddClaims(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsPrincipal),
            };
            var report = new List<ParticipateViewModel>
                {
                    new ParticipateViewModel {FirstName="FirstName",LastName="LastName"},
                    new ParticipateViewModel {FirstName="FirstName",LastName="LastName"},
                 };
            var expectedResponseReport = new ServiceResponse<IEnumerable<ParticipateViewModel>>
            {
                Success = true,
                Data = report,
                Message= "Something went wrong please try after some time."
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponseReport))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetParticipantsByManagerId() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponseReport.Message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        //-------------------NominatePartcipant--------------

     
        [Fact]
        public void NominatePartcipant_RedirectsToIndex_WhenUserDetailsIsNull()
        {
            // Arrange
            int participateId = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns<ServiceResponse<UserDetailsViewModel>>(null); 

            var controller = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var actual = controller.NominatePartcipant(participateId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("GetParticipantsByManagerId", actual.ActionName);

            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60),Times.Once);
        }

        [Fact]
        public void NominatePartcipant_RedirecttoSuccess_WhenUserDetailsAvailable()
        {
            //Arrange
            var participateId = 1;
            var participantDetail = new ServiceResponse<ParticipateViewModel>
            {
                Data = new ParticipateViewModel
                {
                    ParticipantId = participateId,
                    FirstName = "f name 1",
                    LastName = "l name 1",
                    Email = "email 1",
                    JobId = 1
                }

            };
            var gettrainerTopicsByJobId = new ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>
            {
                Data = new List<TrainingProgramDetailJViewModel>
                {
                    new TrainingProgramDetailJViewModel
                    {   
                        TrainerProgramDetailId = 1,
                        StartDate = DateTime.Now,
                        TrainerTopicId = 4,
                        TrainerTopic = new TrainingTopicViewModel()
                        {
                            TrainerTopicId = 4,
                            UserId = 2,
                            TopicId = 1,
                            JobId =1,
                            isTrainingScheduled = true,
                            Topic = new TopicViewModel()
                            {
                                TopicId = 1,
                                TopicName = "Topic name 1"
                            }

                        }
                      
                    },
                     new TrainingProgramDetailJViewModel
                    {
                        TrainerProgramDetailId = 2,
                        StartDate = DateTime.Now,
                        TrainerTopicId = 4,
                        TrainerTopic = new TrainingTopicViewModel()
                        {
                            TrainerTopicId = 5,
                            UserId = 3,
                            TopicId = 2,
                            JobId =1,
                            isTrainingScheduled = true,
                            Topic = new TopicViewModel()
                            {
                                TopicId = 2,
                                TopicName = "Topic name 2"
                            }

                        }

                    },

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

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(participantDetail);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(gettrainerTopicsByJobId);
          

            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.NominatePartcipant(participateId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60),Times.Once);


            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60),Times.Once);
               

        }

        [Fact]
        public void NominatePartcipant_RedirecttoIndex_WhenUserDetailsNotNull()
        {
            // Arrange
            var participateId = 1;
            var participantDetail = new ServiceResponse<ParticipateViewModel>
            {
                Data = new ParticipateViewModel
                {
                    ParticipantId = participateId,
                    FirstName = "f name 1",
                    LastName = "l name 1",
                    Email = "email 1",
                    JobId = 1
                },
                Success = false

            };
            var gettrainerTopicsByJobId = new ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>
            {
                Data = new List<TrainingProgramDetailJViewModel>
                {
                    new TrainingProgramDetailJViewModel
                    {
                        TrainerProgramDetailId = 1,
                        StartDate = DateTime.Now,
                        TrainerTopicId = 4,
                        TrainerTopic = new TrainingTopicViewModel()
                        {
                            TrainerTopicId = 4,
                            UserId = 2,
                            TopicId = 1,
                            JobId =1,
                            isTrainingScheduled = true,
                            Topic = new TopicViewModel()
                            {
                                TopicId = 1,
                                TopicName = "Topic name 1"
                            }

                        }

                    },
                     new TrainingProgramDetailJViewModel
                    {
                        TrainerProgramDetailId = 2,
                        StartDate = DateTime.Now,
                        TrainerTopicId = 4,
                        TrainerTopic = new TrainingTopicViewModel()
                        {
                            TrainerTopicId = 5,
                            UserId = 3,
                            TopicId = 2,
                            JobId =1,
                            isTrainingScheduled = true,
                            Topic = new TopicViewModel()
                            {
                                TopicId = 2,
                                TopicName = "Topic name 2"
                            }

                        }

                    },

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

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(participantDetail);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(gettrainerTopicsByJobId);


            var target = new ManagerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.NominatePartcipant(participateId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("GetParticipantsByManagerId", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);


            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }






    }
}
