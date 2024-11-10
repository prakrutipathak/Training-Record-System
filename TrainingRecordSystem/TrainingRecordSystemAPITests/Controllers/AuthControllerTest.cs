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
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPITests.Controllers
{
    public class AuthControllerTest
    {
        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        //--------------Change password
        [Fact]
        public void ChangePassword_ReturnsOkResponse_WhenChangePasswordSuccessfully()
        {
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "TestOldPassword@123",
                NewPassword = "TestNewPassword@123",
                NewConfirmPassword = "TestNewPassword@123"
            };
            var response = new ServiceResponse<string>
            {
                Success = true,
                Message = "Password changed successfully."
            };
            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ChangePassword(It.IsAny<ChangePasswordDto>())).Returns(response);

            //Act
            var actual = target.ChangePassword(changePasswordDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ChangePassword(It.IsAny<ChangePasswordDto>()), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsBadRequest_WhenPasswordIsNotChanged()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "TestOldPassword@123",
                NewPassword = "TestNewPassword@123",
                NewConfirmPassword = "TestNewPassword@123"
            };
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ChangePassword(It.IsAny<ChangePasswordDto>())).Returns(response);

            // Act
            var actual = target.ChangePassword(changePasswordDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ChangePassword(It.IsAny<ChangePasswordDto>()), Times.Once);
        }


        //-----GetUserDetailById----------

        [Fact]

        public void GetUserDetailById_ReturnsOk()
        {

            var userId = 1;
            var user = new User
            {

                UserId = userId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<UserDetailsDto>
            {
                Success = true,
                Data = new UserDetailsDto
                {
                    UserId = userId,
                    FirstName = user.FirstName
                }
            };

            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUserDetailById(userId)).Returns(response);

            //Act
            var actual = target.GetUserDetailById(userId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserDetailById(userId), Times.Once);
        }

        [Fact]

        public void GetUserDetailById_ReturnsNotFound()
        {

            var userId = 1;
            var user = new User
            {

                UserId = userId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<UserDetailsDto>
            {
                Success = false,
                Data = new UserDetailsDto
                {
                    UserId = userId,
                    FirstName = user.FirstName
                }
            };

            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUserDetailById(userId)).Returns(response);

            //Act
            var actual = target.GetUserDetailById(userId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserDetailById(userId), Times.Once);
        }

    }
}
