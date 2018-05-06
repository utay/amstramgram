namespace Core.Models
{
    public class UserFollower
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long FollowerId { get; set; }
        public User Follower { get; set; }
    }
}
