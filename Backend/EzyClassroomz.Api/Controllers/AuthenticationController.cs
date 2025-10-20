
using System.Security.Claims;
using System.Text;
using EzyClassroomz.Api.Classes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using EzyClassroomz.Library.Repositories;
using EzyClassroomz.Library.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EzyClassroomz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UserRepository _userRepository;

    public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger, UserRepository userRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentAuthentication()
    {
        // get current claims
        var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);

        return Ok(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated ?? false,
            claims = claims
        });
    }

    [HttpGet("restricted")]
    [Authorize(policy: "viewRestricted")]
    public async Task<IActionResult> GetRestricted()
    {
        return Ok("This is a restricted endpoint");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        try
        {
            User? existingUser = await _userRepository.GetUserByName(request.Name, readOnly: true);

            if (existingUser != null)
            {
                return Conflict("User with this username already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword,
                TenantId = request.TenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUser(newUser);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"POST /register failed: {ex}");
            return StatusCode(500, "An error occurred during registration.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {
        try
        {
            User? user = await _userRepository.GetUserByName(request.Name, readOnly: true, includeAuthorizationPolicies: true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            Response.Cookies.Append("jwt", GenerateJwtToken(request.Name, user.AuthorizationPolicies.ToDictionary(p => p.Name, p => "true")), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"]!))
            });

            return Ok(new { message = "Logged in (DEBUG mode)" });

        }
        catch (Exception ex)
        {
            _logger.LogError($"POST /login failed: {ex}");
            return StatusCode(500, "An error occurred during login.");
        }
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
