using EzyClassroomz.Api.Classes;

namespace EzyClassroomz.Testing.Shared
{
    public static class AuthUtilities
    {
        public static async Task RegisterUser(HttpClient client, RegisterRequestDTO registerRequest)
        {
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(registerRequest), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/authentication/register", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to register user. Status code: {response.StatusCode}");
            }
        }

        public static async Task Login(HttpClient client, LoginDTO loginRequest)
        {
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginRequest), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/authentication/login", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to login user. Status code: {response.StatusCode}");
            }
        }
    }
}