using System.ComponentModel.DataAnnotations;

namespace EzyClassroomz.Library.Entities
{
    public class UserAuthorizationPolicy
    {
        [Key]
        public long Id { get; set; }
        public required string Name { get; set; }
        public long UserId { get; set; } = 0;
        public User User { get; set; } = null!;

        public UserAuthorizationPolicy()
        {
        }

        public UserAuthorizationPolicy(string name, User user)
        {
            Name = name;
            UserId = user.Id;
            User = user;
        }
    }
}