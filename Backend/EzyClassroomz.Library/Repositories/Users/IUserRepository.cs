using EzyClassroomz.Library.Entities;

namespace EzyClassroomz.Library.Repositories.Users
{
    public interface IUserRepository
    {
        public Task<User?> GetUserById(long id, bool readOnly = false, bool includeAuthorizationPolicies = false);
        public Task<User?> GetUserByName(string name, bool readOnly = false, bool includeAuthorizationPolicies = false);
        public Task CreateUser(User user);
        public Task UpdateUser(User user);
    }
}