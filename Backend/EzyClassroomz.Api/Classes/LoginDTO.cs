namespace EzyClassroomz.Api.Controllers;

public record LoginDTO
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}