using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Repositories
{
    public class UserFollowerRepository : BaseRepository<UserFollower, long>, IUserFollowerRepository
    {
        public UserFollowerRepository() { }

        public UserFollowerRepository(AmstramgramContext db, ILogger<UserFollowerRepository> logger)
            : base(db, logger)
        {
        }

        public UserFollower Find(long userId, long followerId)
        {
            return _db.Set<UserFollower>()
                .Where(l => l.UserId.Equals(userId)
                    && l.FollowerId.Equals(followerId))
                .FirstOrDefault();
        }

        public async Task<User> GetUser(long id)
        {
            var uf = await Get(id, "User");
            return uf.User;
        }

        public async Task<User> GetFollower(long id)
        {
            var uf = await Get(id, "Follower");
            return uf.Follower;
        }
    }
}
