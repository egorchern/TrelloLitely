using EzyClassroomz.Library.DTO;
using EzyClassroomz.Library.Entities;

namespace EzyClassroomz.Library.Services.Users
{
    public interface IUserService
    {
        // Define user-related service methods here
        public Task<RegisterUserResult> RegisterUser(UserRegisterRequest userRegisterRequest);
        public Task<User?> GetUserByLogin(string name, string password);
    }
}