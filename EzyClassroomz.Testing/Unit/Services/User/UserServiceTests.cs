using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyClassroomz.Library.Repositories.Users;
using EzyClassroomz.Library.Services.Users;
using EzyClassroomz.Library.Entities;
using Moq;
using EzyClassroomz.Library.DTO;

namespace EzyClassroomz.Testing.Unit.Services.User
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IUserRepository> _userServiceMock;
        private const string _testPassword = "Testfsdsdfsdf$1234";
        private EzyClassroomz.Library.Entities.User _testUser = new EzyClassroomz.Library.Entities.User {
            Name = "testuser",
            Email = "testuser@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(_testPassword),
            TenantId = "tenant1"
        };

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserRepository>();
            _userService = new UserService(_userServiceMock.Object);
        }

        [Test]
        public async Task Test_GetUserByLogin_Success()
        {

            _userServiceMock.Setup(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(_testUser);

            // Act
            var user = await _userService.GetUserByLogin(_testUser.Name, _testPassword);

            // Assert
            Assert.That(user != null);
            Assert.That(user!.Name == _testUser.Name);
            _userServiceMock.Verify(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_GetUserByLogin_NotExistingUser()
        {
            _userServiceMock.Setup(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((EzyClassroomz.Library.Entities.User?)null);

            // Act
            var user = await _userService.GetUserByLogin(_testUser.Name, "password");

            // Assert
            Assert.That(user == null);
            _userServiceMock.Verify(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_GetUserByLogin_WrongPassword()
        {
            _userServiceMock.Setup(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(_testUser);

            // Act
            var user = await _userService.GetUserByLogin(_testUser.Name, "wrongpassword");

            // Assert
            Assert.That(user == null);
            _userServiceMock.Verify(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_RegisterUser_Success()
        {
            _userServiceMock.Setup(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((EzyClassroomz.Library.Entities.User?)null);

            // Act
            var result = await _userService.RegisterUser(new UserRegisterRequest(_testUser.Name, _testUser.Email, _testPassword, _testUser.TenantId));

            // Assert
            Assert.That(result.Success == true);
            _userServiceMock.Verify(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            _userServiceMock.Verify(repo => repo.CreateUser(It.Is<EzyClassroomz.Library.Entities.User>(u =>
                u.Name == _testUser.Name &&
                u.Email == _testUser.Email &&
                u.TenantId == _testUser.TenantId &&
                BCrypt.Net.BCrypt.Verify(_testPassword, u.PasswordHash, false, BCrypt.Net.HashType.SHA384)
            )), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_RegisterUser_ExistingUsername()
        {
            // Arrange

            _userServiceMock.Setup(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _userService.RegisterUser(new UserRegisterRequest(_testUser.Name, _testUser.Email, _testPassword, _testUser.TenantId));

            // Assert
            Assert.That(result.Success == false);
            Assert.That(result.Error == RegisterUserError.UserAlreadyExists);
            _userServiceMock.Verify(repo => repo.GetUserByName(_testUser.Name, It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_RegisterUser_WeakPassword()
        {
            // Act
            var result = await _userService.RegisterUser(new UserRegisterRequest(_testUser.Name, _testUser.Email, "weak", _testUser.TenantId));

            // Assert
            Assert.That(result.Success == false);
            Assert.That(result.Error == RegisterUserError.WeakPassword);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_RegisterUser_MissingRequiredFields()
        {
            // Act
            var result = await _userService.RegisterUser(new UserRegisterRequest("", _testUser.Email, _testPassword, _testUser.TenantId));

            // Assert
            Assert.That(result.Success == false);
            Assert.That(result.Error == RegisterUserError.MissingRequiredFields);
            _userServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_RegisterUser_InvalidEmail()
        {
            // Act
            var result = await _userService.RegisterUser(new UserRegisterRequest(_testUser.Name, "invalidemail", _testPassword, _testUser.TenantId));

            // Assert
            Assert.That(result.Success == false);
            Assert.That(result.Error == RegisterUserError.InvalidEmail);
            _userServiceMock.VerifyNoOtherCalls();
        }
    }
}
