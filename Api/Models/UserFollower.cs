namespace Api.Models
{
    public class UserFollower
    {
        public long Id { get; set; }
        public User User { get; set; }
        public User Follower { get; set; }
    }
}
