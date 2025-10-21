namespace EzyClassroomz.Library.DTO
{
    public record UserRegisterRequest
    (
        string Name,
        string Email,
        string Password,
        string TenantId
    );
}
