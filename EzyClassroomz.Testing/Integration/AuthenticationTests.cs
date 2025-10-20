using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EzyClassroomz.Api.Classes;
using EzyClassroomz.Testing.Shared;

namespace EzyClassroomz.Testing.Integration
{
    [TestFixture]
    public class AuthenticationTests
    {
        private CustomWebApplicationFactory _appFactory;
        private HttpClient _client;

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
            string password = "newpass";
            string email = "newuser@example.com";
            string tenant = "testtenant";

            RegisterRequestDTO registerRequest = new RegisterRequestDTO(username, email, password, tenant);

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
            string password = "anotherpass";
            string wrongPassword = password + "wrong";
            string email = "anotheruser@example.com";
            string tenant = "testtenant";

            RegisterRequestDTO registerRequest = new RegisterRequestDTO(username, email, password, tenant);
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
            string password = "existpass";
            string email = "existinguser@example.com";
            string tenant = "testtenant";

            RegisterRequestDTO registerRequest = new RegisterRequestDTO(username, email, password, tenant);
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
    }
}
