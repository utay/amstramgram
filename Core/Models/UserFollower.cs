using Core.Data;

namespace Core.Models
{
    public class UserFollower : IEntity<long>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

        public long FollowerId { get; set; }
        public User Follower { get; set; }
    }
}
