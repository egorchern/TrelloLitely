using EzyClassroomz.Library.Validators;

namespace EzyClassroomz.Library.Services.Users
{
    public enum RegisterUserError
    {
        UserAlreadyExists,
        MissingRequiredFields,
        InvalidEmail,
        WeakPassword,
    }

    public record RegisterUserResult
    {
        public bool Success { get; init; }
        public RegisterUserError? Error { get; init; }
        public PasswordComplexityResult? PasswordComplexityResult { get; init; }
    }
}