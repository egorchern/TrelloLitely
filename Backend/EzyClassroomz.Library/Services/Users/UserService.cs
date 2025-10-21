using EzyClassroomz.Library.DTO;
using EzyClassroomz.Library.Entities;
using EzyClassroomz.Library.Repositories.Users;
using EzyClassroomz.Library.Validators;

namespace EzyClassroomz.Library.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterUserResult> RegisterUser(UserRegisterRequest userRegisterRequest)
        {
            RegisterUserResult registerUserResult = new RegisterUserResult();

            if (!BasicValidateNewUser(userRegisterRequest, out RegisterUserError? error, out PasswordComplexityResult? passwordComplexityResult))
            {
                registerUserResult = new RegisterUserResult
                {
                    Success = false,
                    Error = error,
                    PasswordComplexityResult = passwordComplexityResult
                };

                return registerUserResult;
            }

            User? existingUser = await _userRepository.GetUserByName(userRegisterRequest.Name, readOnly: true);

            if (existingUser != null)
            {
                registerUserResult = new RegisterUserResult
                {
                    Success = false,
                    Error = RegisterUserError.UserAlreadyExists
                };

                return registerUserResult;
            }

            // Validation passed, create the user

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterRequest.Password);

            User newUser = new User
            {
                Name = userRegisterRequest.Name,
                Email = userRegisterRequest.Email,
                PasswordHash = hashedPassword,
                TenantId = userRegisterRequest.TenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUser(newUser);

            registerUserResult = new RegisterUserResult
            {
                Success = true,
                Error = null
            };

            return registerUserResult;
        }

        public async Task<User?> GetUserByLogin(string username, string password)
        {
            User? user = await _userRepository.GetUserByName(username, readOnly: true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
        
        private bool BasicValidateNewUser(UserRegisterRequest userRegisterRequest, out RegisterUserError? error, out PasswordComplexityResult? passwordComplexityResult)
        {
            // Basic validation
            error = null;
            passwordComplexityResult = null;

            if (string.IsNullOrWhiteSpace(userRegisterRequest.Name) ||
                string.IsNullOrWhiteSpace(userRegisterRequest.Email) ||
                string.IsNullOrWhiteSpace(userRegisterRequest.Password) ||
                string.IsNullOrWhiteSpace(userRegisterRequest.TenantId))
            {
                error = RegisterUserError.MissingRequiredFields;
                return false;
            }

            PasswordComplexityRequirements passwordComplexityRequirements = new PasswordComplexityRequirements();
            passwordComplexityResult = PasswordComplexityValidator.Validate(userRegisterRequest.Password, passwordComplexityRequirements);

            if (passwordComplexityResult != PasswordComplexityResult.Valid)
            {
                error = RegisterUserError.WeakPassword;
                return false;
            }

            var emailAddressAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();

            if (!emailAddressAttribute.IsValid(userRegisterRequest.Email))
            {
                error = RegisterUserError.InvalidEmail;
                return false;
            }

            return true;
        }
    }
}