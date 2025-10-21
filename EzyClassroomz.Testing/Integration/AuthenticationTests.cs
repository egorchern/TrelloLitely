using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EzyClassroomz.Api.Classes;
using EzyClassroomz.Library.DTO;
using EzyClassroomz.Testing.Shared;

namespace EzyClassroomz.Testing.Integration
{
    [TestFixture]
    public class AuthenticationTests
    {
        private CustomWebApplicationFactory _appFactory;
        private HttpClient _client;
        private const string STRONG_PASSWORD = "lej8o#8E$0Xq";

        [SetUp]
        public void Setup()
        {
            _appFactory = new CustomWebApplicationFactory();
            _client = _appFactory.CreateClient();
            _appFactory.InitializeDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _appFactory.Dispose();
        }

        [Test]
        public async Task Test_GetNonAuthenticated()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/api/authentication");

            // Assert
            Assert.That(response.StatusCode == System.Net.HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<dynamic>(content);
            Assert.That((bool)json?.GetProperty("isAuthenticated")?.GetBoolean() == false);
        }

        [Test]
        public async Task Test_LoginNonExistingUser()
        {
            // Arrange
            LoginDTO loginRequest = new LoginDTO("nonexistent", "pass");
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await AuthUtilities.Login(_client, loginRequest);
            });
        }

        [Test]
        public async Task Test_SuccessfullRegisterAndLogin()
        {
            // Arrange
            string username = "newuser";
            string password = STRONG_PASSWORD;
            string email = "newuser@example.com";
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, password, tenant);

            Assert.DoesNotThrowAsync(async () =>
            {
                await AuthUtilities.RegisterUser(_client, registerRequest);
            });

            // Act - Login
            LoginDTO loginRequest = new LoginDTO(username, password);
            Assert.DoesNotThrowAsync(async () =>
            {
                await AuthUtilities.Login(_client, loginRequest);
            });
        }

        [Test]
        public async Task Test_SuccessfullRegister_AndLoginWithWrongPassword()
        {
            // Arrange
            string username = "anotheruser";
            string password = STRONG_PASSWORD;
            string wrongPassword = password + "wrong";
            string email = "anotheruser@example.com";
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, password, tenant);
            Assert.DoesNotThrowAsync(async () =>
            {
                await AuthUtilities.RegisterUser(_client, registerRequest);
            });

            // Act - Login with wrong password
            LoginDTO loginRequest = new LoginDTO(username, wrongPassword);
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await AuthUtilities.Login(_client, loginRequest);
            });
        }

        [Test]
        public async Task Test_RegisterExistingUser()
        {
            // Arrange
            string username = "existinguser";
            string password = STRONG_PASSWORD;
            string email = "existinguser@example.com";
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, password, tenant);
            Assert.DoesNotThrowAsync(async () =>
            {
                await AuthUtilities.RegisterUser(_client, registerRequest);
            });

            // Act - Try to register the same user again
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await AuthUtilities.RegisterUser(_client, registerRequest);
            });
        }

        [Test]
        public async Task Test_RegisterUser_WeakPassword()
        {
            // Arrange
            string username = "weakpassworduser";
            string weakPassword = "123";
            string email = "existinguser@example.com";
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, weakPassword, tenant);
            // Act - Try to register the user with weak password
            var response = await AuthUtilities.RegisterUserReturningResponse(_client, registerRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.That(content.IndexOf("weak password", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [Test]
        public async Task Test_RegisterUser_MissingFields()
        {
            // Arrange
            string username = ""; // Missing username
            string password = STRONG_PASSWORD;
            string email = "missinguser@example.com";
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, password, tenant);
            // Act - Try to register the user with missing fields
            var response = await AuthUtilities.RegisterUserReturningResponse(_client, registerRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.That(content.IndexOf("missing required", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [Test]
        public async Task Test_RegisterUser_InvalidEmail()
        {
            // Arrange
            string username = "invalidemailuser";
            string password = STRONG_PASSWORD;
            string email = "invalid-email"; // Invalid email format
            string tenant = "testtenant";

            UserRegisterRequest registerRequest = new UserRegisterRequest(username, email, password, tenant);
            // Act - Try to register the user with invalid email
            var response = await AuthUtilities.RegisterUserReturningResponse(_client, registerRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.That(content.IndexOf("invalid email", StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
