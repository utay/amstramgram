using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserFollowerRepository : BaseRepository<UserFollower, long>, IUserFollowerRepository
    {
        public UserFollowerRepository() { }

        public UserFollowerRepository(AmstramgramContext db, ILogger<UserFollowerRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<User> GetUser(long id)
        {
            var userFollower = await Get(id, "User");
            return userFollower.User;
        }

        public async Task<User> GetFollower(long id)
        {
            var userFollower = await Get(id, "Follower");
            return userFollower.Follower;
        }

        public Task<UserFollower> Find(long userId, long followerId)
        {
            return _db.Set<UserFollower>().FindAsync(userId, followerId);
        }
    }
}
