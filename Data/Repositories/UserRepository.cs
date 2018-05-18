using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;

namespace Data.Repositories
{
    public class UserRepository : BaseRepository<User, long>, IUserRepository
    {
        public UserRepository() { }

        public UserRepository(AmstramgramContext db, ILogger<UserRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<ICollection<Picture>> GetPictures(long id)
        {
            var user = await Get(id, "Pictures");
            return user.Pictures;
        }

        public async Task<ICollection<UserFollower>> GetFollowers(long id)
        {
            var user = await Get(id, "Followers");
            return user.Followers;
        }

        public async Task<ICollection<UserFollower>> GetFollowing(long id)
        {
            var user = await Get(id, "Following");
            return user.Following;
        }

        public async Task<User> GetFromEmail(string email)
        {
            User user = (await GetAll()).Find(u => u.Email == email);
            return user;
        }

        public async Task<User> GetFromAccessToken(string accessToken)
        {
            User user = (await GetAll()).Find(u => u.Password == accessToken);
            return user;
        }
    }
}
