using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Data;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPITests.Repositories
{
    public class AuthRepositoryTest
    {

        [Fact]
        public void UpdateUser_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                LoginId = "loginId",
                Email = "xyz@gmail.com",
                
            };
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateLoginBit(user);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(p => p.Update(user), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateUser_ReturnsFalse()
        {
            // Arrange
            var loginId = "non_existing_login_id";
            var email = "non_existing_email@example.com";
            var usersData = new List<User>
            { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new AuthRepository(mockAppDbContext.Object);
            User user = null;

            //var target = new AuthRepository(mockDbContext.Object);

            // Act
            var actual = target.UpdateLoginBit(user);

            // Assert
            Assert.False(actual);
        }
        [Fact]
        public void ValidateUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var username = "existing_username";
            var userData = new List<User>
        {
            new User { LoginId = username },
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockDbContext.Object);

            // Act
            var result = target.ValidateUser(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.LoginId);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression, Times.Once);
        }

        [Fact]
        public void ValidateUser_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "non_existing_username";
            var userData = new List<User>
            { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockDbContext.Object);

            // Act
            var result = target.ValidateUser(username);

            // Assert
            Assert.Null(result);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Provider,Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(m => m.Expression,Times.Once);
            
       
        }

        //----------GetUserDetailById------
        [Fact]
        public void GetUserDetailById_WhenContactIsNull()
        {
            //Arrange
            var id = 1;
            var user = new List<User>().AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(user.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(user.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.GetUserDetailById(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);

        }

        [Fact]
        public void GetUserDetailById_WhenContactIsNotNull()
        {
            //Arrange
            var id = 1;
            var users = new List<User>()
            {
              new User { UserId = 1, FirstName = "Firstname 1", LastName = "lastname 1",
                LoginId = "loginid1",
                Email = "email1@gmail.com",
                Role = 1,
                JobId = 2 },
                new User { UserId = 2, FirstName = "Firstname 2",
                LastName = "lastname 2",
                LoginId = "loginid2",
                Email = "email2@gmail.com",
                Role = 2,
                JobId = 3 },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.GetUserDetailById(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);

        }

    }
}
