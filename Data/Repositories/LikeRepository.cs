using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LikeRepository : BaseRepository<Like, long>, ILikeRepository
    {
        public LikeRepository() { }

        public LikeRepository(AmstramgramContext db, ILogger<LikeRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<Picture> GetPicture(long id)
        {
            var like = await Get(id, "Picture");
            return like.Picture;
        }

        public async Task<User> GetUser(long id)
        {
            var like = await Get(id, "User");
            return like.User;
        }
    }
}
