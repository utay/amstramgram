using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserFollowerRepository : BaseRepository<UserFollower, long>, IUserFollowerRepository
    {
        public UserFollowerRepository() { }

        public UserFollowerRepository(AmstramgramContext db, ILogger<UserFollowerRepository> logger)
            : base(db, logger)
        {
        }

        public Task<UserFollower> Find(long userId, long followerId)
        {
            return _db.Set<UserFollower>().FindAsync(userId, followerId);
        }
    }
}
