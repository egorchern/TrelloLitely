
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EzyClassroomz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetCurrentAuthentication()
    {
        // get current claims
        var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
        return Ok(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated ?? false,
            claims = claims
        });
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginDTO request)
    {
#if DEBUG
        Response.Cookies.Append("jwt", GenerateJwtToken(request.Username, new Dictionary<string, string>
        {
            ["canViewStats"] = "true"
        }), new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(60)
        });
        return Ok(new { message = "Logged in (DEBUG mode)" });
        #endif
        return Unauthorized();
    }

    private string GenerateJwtToken(string username, Dictionary<string, string> claims)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Dictionary<string, object> claimsDict = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = username,
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString()
        };

        foreach (var claim in claims)
        {
            claimsDict[claim.Key] = claim.Value;
        }

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claimsDict.Select(c => new Claim(c.Key, c.Value.ToString()!))),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"]!)),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = credentials
        };

        JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }
}
