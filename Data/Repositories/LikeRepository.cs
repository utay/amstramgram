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

        public Task<Like> Find(long userId, long pictureId)
        {
            return _db.Set<Like>().FindAsync(userId, pictureId);
        }

        public void Delete(long userId, long pictureId)
        {
            var entity = new Like { UserId = userId, PictureId = pictureId };
            _db.Set<Like>().Remove(entity);
        }

        public async Task<User> GetUser(long id)
        {
            var comment = await Get(id, "User");
            return comment.User;
        }
    }
}
