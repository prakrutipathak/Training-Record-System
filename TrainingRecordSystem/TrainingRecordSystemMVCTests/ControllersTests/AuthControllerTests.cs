using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemMVC.Controllers;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVCTests.ControllersTests
{
    public class AuthControllerTests
    {

        //-----------Login--------
        [Fact]
        public void Login_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Login_ModelIsInvalid()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "Username is required");
            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void Login_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.Null(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        [Fact]
        public void Login_Success_RedirectToAction()
        {
            //Arrange
            var loginViewModel = new LoginViewModel { UserName = "loginid", Password = "Password" };
            var mockToken = "mockToken";
            var mockuserId= "2";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("userId", mockuserId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,

            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
             .Returns(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var claims = new[]
            {
                new Claim("UserId", mockuserId),

            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockJwtTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockResponseCookie.Verify(c => c.Append("userId", mockuserId, It.IsAny<CookieOptions>()), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Exactly(2));
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Exactly(2));
            Assert.True(target.ModelState.IsValid);

        }

        [Fact]
        public void Login_RedirectToAction_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            ServiceResponse<LoginViewModel> expectedResponseContent = null;

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponseContent))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedErrorResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };



            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.Null(actual);
            Assert.Equal("Something went wrong.Please try after sometime.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        //----------------LogOut--------------
        [Fact]
        public void Logout_DeleteJwtToken()
        {
            //Arrange
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Delete("jwtToken"));
            mockResponseCookie.Setup(c => c.Delete("userId"));
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            var mockHttpResponse = new Mock<HttpResponse>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.LogOut() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockResponseCookie.Verify(c => c.Delete("jwtToken"), Times.Once);
            mockResponseCookie.Verify(c => c.Delete("userId"), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Exactly(2));
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Exactly(2));
        }

        //-------------Change Password--------------
        [Fact]
        public void ChangePassword_ReturnsView()
        {
            // Arrange
            var updatePasswordViewModel = new ChangePasswordViewModel { LoginId = "user", OldPassword = "password@123", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.ChangePassword() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangePasswordViewModel>(result.Model);
            var model = result.Model as ChangePasswordViewModel;
            Assert.Equal("user", model.LoginId);
        }

        //HttpPost chnage password
        [Fact]
        public void ChangePassword_ModelIsInvalid()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { OldPassword = "Password@1234", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("LoginId", "User name is required.");
            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(changePasswordViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void ChangePassword_RedirectToLogOutWithPasswordUpdatedSuccessfullyMessage_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { LoginId = "user", OldPassword = "Password@1234", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = string.Empty;
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Equal("Password updated successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void ChangePassword_RedirectToLogOut_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { LoginId = "user", OldPassword = "Password@1234", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Success";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Equal("Auth", actual.ControllerName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void ChangePassword_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { LoginId = "user", OldPassword = "Password@1234", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Invalid username or other details";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ChangePassword", actual.ActionName);
            Assert.Equal("Invalid username or other details", target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void ChangePassword_WhenSomethingWentWrong()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { LoginId = "user", OldPassword = "Password@1234", NewPassword = "password@123", NewConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
           var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ChangePassword", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
   


        [Fact]
        public void ChangePassword_InvalidModelState_ReturnsView()
        {
            // Arrange
            var viewModel = new ChangePasswordViewModel();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            var controller = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object);
            controller.ModelState.AddModelError("Key", "Error Message");

            // Act
            var result = controller.ChangePassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(viewModel, result.Model); // Verify that the same viewModel is returned to the view
        }

        [Fact]
        public void ChangePassword_ApiFailure_ReturnsRedirectToAction()
        {
            // Arrange
            var viewModel = new ChangePasswordViewModel();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            var errorResponseContent = JsonConvert.SerializeObject(new ServiceResponse<string> { Message = "Success message from API" });
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorResponseContent) };

            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(x => x.PutHttpResponseMessage(It.IsAny<string>(), It.IsAny<ChangePasswordViewModel>(), It.IsAny<HttpRequest>()))
                                 .Returns(responseMessage);

            var controller = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockJwtTokenHandler.Object);
            controller.ModelState.AddModelError("Key", "Success Message");

            // Act
            var result = controller.ChangePassword(viewModel) as RedirectToActionResult;

            // Assert
            Assert.Null(result);
        }
    }
}
