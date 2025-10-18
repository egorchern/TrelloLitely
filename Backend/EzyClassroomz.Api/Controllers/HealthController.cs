using Microsoft.AspNetCore.Mvc;

namespace EzyClassroomz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // env var output
        string running_environment = Environment.GetEnvironmentVariable("custom_environment") ?? "Not Set";
        return Ok(new { status = $"{running_environment} healthy", timestamp = DateTime.UtcNow });
    }
}
