using EzyClassroomz.Library.Data;
using EzyClassroomz.Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzyClassroomz.Library.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(long id, bool readOnly = false, bool includeAuthorizationPolicies = false)
        {
            return await GetCommonSingleUserQuery(readOnly, includeAuthorizationPolicies)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByName(string name, bool readOnly = false, bool includeAuthorizationPolicies = false)
        {
            return await GetCommonSingleUserQuery(readOnly, includeAuthorizationPolicies)
                .Where(u => u.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            var tracked = _context.ChangeTracker
                .Entries<User>()
                .FirstOrDefault(e => e.Property("Id").CurrentValue?.Equals(user.Id) == true)
                ?.Entity;

            if (tracked == null)
            {
                tracked = await GetUserById(user.Id);
            }

            if (tracked == null)
            {
                throw new InvalidOperationException($"User with Id {user.Id} not found in the database.");
            }

            _context.Entry(tracked!).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
        }
        
        private IQueryable<User> GetCommonSingleUserQuery(bool readOnly, bool includeAuthorizationPolicies)
        {
            IQueryable<User> query = _context.Users;

            if (readOnly)
                query = query.AsNoTracking();

            if (includeAuthorizationPolicies)
                query = query.Include(u => u.AuthorizationPolicies);

            return query;
        }
    }
}