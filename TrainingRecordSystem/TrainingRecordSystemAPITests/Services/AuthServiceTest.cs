using Moq;
using System;
using System.Collections.Generic;
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
    public class AuthServiceTest:IDisposable
    {
        private readonly Mock<IAuthRepository> mockAuthRepository;
        private readonly Mock<IPasswordService> mockTokenService;
        public AuthServiceTest() 
        {
            mockAuthRepository= new Mock<IAuthRepository>();
            mockTokenService= new Mock<IPasswordService>();
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after some time", result.Message);

        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username"
            };
         
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user login id or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);


        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var user = new User
            {
                UserId = 1,
                LoginId = "username",
                Email = "abc@gmail.com"
            };
           
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user login id or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);


        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var user = new User
            {
                UserId = 1,
                LoginId = "username",
                Email = "abc@gmail.com"
            };
          
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockTokenService.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockTokenService.Setup(repo => repo.CreateToken(user)).Returns("");

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockTokenService.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockTokenService.Verify(repo => repo.CreateToken(user), Times.Once);


        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_WhenThrowException()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
          
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Throws(new Exception());

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
           
        }

        // ChangePassword
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenExistingUerIsNull()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Invalid username or password"
            };

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns<User>(null);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenNewAndOldPasswordIsSame()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "NewTest@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Old password and new password can not be same."
            };

            var user = new User()
            {
                UserId = 1,
                FirstName = "test",
                LoginId = changePasswordDto.LoginId,
            };

            mockTokenService.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns(user);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenVerifyPasswordHashFails()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Old password is incorrect."
            };

            var user = new User()
            {
                UserId = 1,
                FirstName = "test",
                LoginId = changePasswordDto.LoginId,
            };

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);
            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenUpdationFails()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };

            var user = new User()
            {
                UserId = 1,
                FirstName = "test",
                LoginId = changePasswordDto.LoginId,
            };

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(p => p.UpdateLoginBit(It.IsAny<User>())).Returns(false);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(p => p.UpdateLoginBit(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsSuccessMessage_WhenUpdatedSuccessfully()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = true,
                Message = "Successfully changed password, Please signin!"
            };

            var user = new User()
            {
                UserId = 1,
                FirstName = "test",
                LoginId = changePasswordDto.LoginId,
            };

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(p => p.UpdateLoginBit(It.IsAny<User>())).Returns(true);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(p => p.UpdateLoginBit(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenPasswordStrengthIsNotProper()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "NewTest@123",
                NewPassword = "test",
                NewConfirmPassword = "test"
            };
            string message = "Mininum password length should be 8\r\nPassword should be alphanumeric\r\nPassword should contain special characters\r\n";
            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = message
            };
            var user = new User()
            {
                UserId = 1,
                FirstName = "test",
                LoginId = changePasswordDto.LoginId,
            };
            mockTokenService.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockTokenService.Setup(x => x.CheckPasswordStrength(changePasswordDto.NewPassword)).Returns(message);
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Returns(user);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockTokenService.Verify(x => x.CheckPasswordStrength(changePasswordDto.NewPassword), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenDtoIsNull()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto() { };
            changePasswordDto = null;
            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_WhenThrowException()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.LoginId)).Throws(new Exception());

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }


        //--------GetUserDetailById------

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void GetContactsById_ReturnEmpty_WhenNoContactExist()
        {
            //Arrange
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);
            //Act
            var actual = target.GetUserDetailById(1);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found !.", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void GetContactsById_ReturnsContact_WhenContactsExist()
        {
            //Arrange
            var user = new User
            {
                UserId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                LoginId = "loginid",
                Email = "email@gmail.com",
                Role = 1,
                JobId = 2

            };

        
            mockAuthRepository.Setup(c => c.GetUserDetailById(user.UserId)).Returns(user);
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);
            //Act
            var actual = target.GetUserDetailById(user.UserId);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockAuthRepository.Verify(c => c.GetUserDetailById(user.UserId), Times.Once);



        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void GetContactsById_ThrowException()
        {
            //Arrange
           
            mockAuthRepository.Setup(c => c.GetUserDetailById(1)).Throws(new Exception());
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);
            //Act
            var actual = target.GetUserDetailById(1);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
        }

        public void Dispose()
        {
            mockAuthRepository.VerifyAll();
            mockTokenService.VerifyAll();
        }

    }
}
