namespace EzyClassroomz.Api.Classes;

public record RegisterRequestDTO
(
    string Name,
    string Email,
    string Password,
    string TenantId
);