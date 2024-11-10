using AutoFixture;
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
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemMVC.Controllers;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;
using Xunit.Sdk;

namespace TrainingRecordSystemMVCTests.ControllersTests
{
    public class TrainerControllerTests
    {

        //------------------Index-----------------------
        [Fact]
        public void Index_ReturnViewWithData()
        {
            //Arrange
            int page = 1;
            int pageSize = 6;

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

            var fixture = new Fixture();
            var expectedProducts = fixture.Create<List<TopicByPaginationViewModel>>();


            var expectedServiceResponse = new ServiceResponse<IEnumerable<TopicByPaginationViewModel>>
            {
                Data = expectedProducts,
                Message = "",
                Success = true
            };

            var expectedCount = new ServiceResponse<int>()
            {
                Data = expectedProducts.Count(),
                Success = true
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicByPaginationViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), It.IsAny<Object>(), 60)).Returns(expectedServiceResponse);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedCount);


            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },
            };

            //Act
            var actual = target.Index(page, pageSize) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedProducts, actual.Model);

            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicByPaginationViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Index_ReturnEmptyList()
        {
            //Arrange
            int page = 1;
            int pageSize = 6;

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

           


            var expectedServiceResponse = new ServiceResponse<IEnumerable<TopicByPaginationViewModel>>
            {
                Data = { },
                Message = "",
                Success = false
            };

            var expectedCount = new ServiceResponse<int>()
            {
                Data = 0,
                Success = true
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicByPaginationViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), It.IsAny<Object>(), 60)).Returns(expectedServiceResponse);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedCount);


            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },
            };

            //Act
            var actual = target.Index(page, pageSize) as ViewResult;

            //Assert
            Assert.NotNull(actual);
           Assert.Equal(3, actual.ViewData.Count());
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicByPaginationViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


        //-------------Nominated Participants List--------------
        [Fact]
        public void GetNominatedParticipants_ReturnsParticipants()
        {
            // Arrange

            var expectedContacts = new List<GetNominatedParticipateViewModel>
            {
                new GetNominatedParticipateViewModel
                {
                    NomiationId = 1,
                    ModePreference = "Online",
                    TopicId = 1,
                    Topic = new TopicViewModel(),
                    ParticipateId = 1,
                    Participate = new ParticipateViewModel()

                },
                 new GetNominatedParticipateViewModel
                {
                    NomiationId = 1,
                    ModePreference = "Online",
                    TopicId = 1,
                    Topic = new TopicViewModel(),
                    ParticipateId = 1,
                    Participate = new ParticipateViewModel()

                }
            };

        var pageSize = 4;


        var mockHttpClientService = new Mock<IHttpClientService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockHttpRequest = new Mock<HttpRequest>();

        var countResponse = new ServiceResponse<int> { Data = expectedContacts.Count };
        var response = new ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>> { Success = true, Data = expectedContacts };

        mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
        mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(response);
        mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);

        var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object,
            }
        };

        // Act
        var actual = target.GetNominatedParticipants(1, pageSize,"default") as ViewResult;

        // Assert
        Assert.NotNull(actual);
            Assert.True(actual.ViewData.ContainsKey("PageSize"));
            Assert.Equal(expectedContacts, actual.Model as List<GetNominatedParticipateViewModel>);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.AtLeastOnce);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


    [Fact]
    public void GetNominatedParticipants_ReturnsError_WhenNoParticipantsExists()
    {
        // Arrange

        var expectedContacts = new List<GetNominatedParticipateViewModel>();
        var pageSize = 6;

        var mockHttpClientService = new Mock<IHttpClientService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockHttpRequest = new Mock<HttpRequest>();

        var countResponse = new ServiceResponse<int> { Data = expectedContacts.Count };
        var response = new ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>> { Success = false };

        mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
        mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(response);
        mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);

        var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object,
            }
        };

        // Act
        var actual = target.GetNominatedParticipants(1, pageSize, "default") as ViewResult;

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.ViewData.ContainsKey("PageSize"));
        Assert.Equal(expectedContacts, actual.Model as List<GetNominatedParticipateViewModel>);
        mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.AtLeastOnce);
        mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
            It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
    }

        //----------------AddProgramDetails Get----------------
        [Fact]
        public void Create_ReturnsView()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
           
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.AddProgramDetails() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

        }

        //-----------------------AddProgramDetails Post--------------------

        [Fact]
        public void AddProgramDetails_DetailsSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var viewModel = new AddProgramDetailsViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                StartTime = "",
                EndTime = "",
                ModePreference = "Online",
                TargetAudience = " ",
                TrainerTopicId = 1

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
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddProgramDetails(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void AddProgramDetails_DetailsSavedSuccessfully_ReturnsViews()
        {
            //Arrange
            var viewModel = new AddProgramDetailsViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                StartTime = "",
                EndTime = "",
                ModePreference = "Online",
                TargetAudience = " ",
                TrainerTopicId = 1

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            string successMessage = null;
            var expectedServiceResponse = new ServiceResponse<AddProgramDetailsViewModel>
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
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },
            };

            //Act
            var actual = target.AddProgramDetails(viewModel) as ViewResult;
            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void AddProgramDetails_DetailsFailedToSave_RedirectToAction_WithInvalidData()
        {
            //Arrange
            var viewModel = new AddProgramDetailsViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                StartTime = "",
                EndTime = "",
                ModePreference = "Online",
                TargetAudience = " ",
                TrainerTopicId = 1

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

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddProgramDetails(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void AddProgramDetails_ReturnsSomethingWentWrong_ReturnView()
        {
            //Arrange
         
            var viewModel = new AddProgramDetailsViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong, please try after sometime.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.AddProgramDetails(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        //------------GetProgramDetails----------------
        [Fact]
        public void GetProgramDetails_ReturnsViewWhenDetailsExists()
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

            var fixture  = new Fixture();
            var programDetails = fixture.Create<TrainingProgramDetailsViewModel>();

            var expectedServiceReponse = new ServiceResponse<TrainingProgramDetailsViewModel>()
            {
                Message = "",
                Data = programDetails,
                Success = true
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceReponse))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetProgramDetails(programDetails.TrainerTopic.TopicId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(programDetails.ToString(), actual.Model.ToString());
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void GetProgramDetails_ReturnsViewWhenDetailsAreNull()
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

            var expectedServiceReponse = new ServiceResponse<TrainingProgramDetailsViewModel>()
            {
                Message = "errorMessage",
                Data = null,
                Success = true
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceReponse))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetProgramDetails(1) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceReponse.Message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void GetProgramDetails_ReturnsViewWithBadRequest()
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

            var expectedServiceReponse = new ServiceResponse<TrainingProgramDetailsViewModel>()
            {
                Message = "errorMessage",
                Data = null,
                Success = true
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceReponse))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetProgramDetails(1) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceReponse.Message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void GetProgramDetails_ReturnsViewWithBadRequest_errorResponseNull()
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

            var expectedServiceReponse = new ServiceResponse<TrainingProgramDetailsViewModel>()
            {
                Message = "errorMessage",
                Data = null,
                Success = true
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
            //Act
            var actual = target.GetProgramDetails(1) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try after sometime.", target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        //------------UpdateProgram details get----------------
        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccess()
        {

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


            var fixture = new Fixture();
            var viewModel = fixture.Create<TrainingProgramDetailsViewModel>();


            var expectedServiceResponse = new ServiceResponse<TrainingProgramDetailsViewModel>
            {
                Data = viewModel,
                Success = true
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");



            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel.TrainerTopic.TopicId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccess_WhenDataIsNULL()
        {

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


            var fixture = new Fixture();
            var viewModel = fixture.Create<TrainingProgramDetailsViewModel>();


            var expectedServiceResponse = new ServiceResponse<TrainingProgramDetailsViewModel>
            {
                Data = null,
                Success = true,
                Message = "errorMessage"
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");



            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel.TrainerTopic.TopicId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName); ;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsBadReuest_WhenErrorResponseNotNull()
        {

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


            var fixture = new Fixture();
            var viewModel = fixture.Create<TrainingProgramDetailsViewModel>();


            var expectedServiceResponse = new ServiceResponse<TrainingProgramDetailsViewModel>
            {
                Data = viewModel,
                Success = true,
                Message = "errorMessage"
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");



            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel.TrainerTopic.TopicId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName); ;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsbadRequest_WhenErrorResponseIsNULL()
        {

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


            var fixture = new Fixture();
            var viewModel = fixture.Create<TrainingProgramDetailsViewModel>();



            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");



            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel.TrainerTopic.TopicId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try after sometime.", target.TempData["ErrorMessage"]);
            Assert.Equal("Index", actual.ActionName); ;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        //------------Update Program details postvb ----------------

        [Fact]
        public void UpdateProgramDetails_UpdatedSuccessfully_ReturnView()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var fixture = new Fixture();
            var viewModel = fixture.Create<UpdateProgramDetailsViewModel>();

         
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedServiceResponse = new ServiceResponse<UpdateProgramDetailsViewModel>
            {
                Data = viewModel,
                Message = "Updated successfully",
                Success = true
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void UpdateProgramDetails_UpdatedSuccessfully_RedirectToAction()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var fixture = new Fixture();
            var viewModel = fixture.Create<UpdateProgramDetailsViewModel>();


            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedServiceResponse = new ServiceResponse<UpdateProgramDetailsViewModel>
            {
                Data = null,
                Message = "Updated successfully",
                Success = true
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["successMessage"]);
            Assert.Equal("Index", actual.ActionName); ;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void UpdateProgramDetails_ReturnBadRequest_ErrorResponseNull()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var fixture = new Fixture();
            var viewModel = fixture.Create<UpdateProgramDetailsViewModel>();


            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedServiceResponse = new ServiceResponse<UpdateProgramDetailsViewModel>
            {
                Data = viewModel,
                Message = "Error",
                Success = true
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void UpdateProgramDetails_ReturnBadRequest_RedirectToAction()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var fixture = new Fixture();
            var viewModel = fixture.Create<UpdateProgramDetailsViewModel>();


            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


          

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new TrainerController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateProgramDetails(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime.", target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
    }
}
