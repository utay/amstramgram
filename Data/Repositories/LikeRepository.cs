using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories
{
    public class LikeRepository : BaseRepository<Like, long>, ILikeRepository
    {
        public LikeRepository() { }

        public LikeRepository(AmstramgramContext db, ILogger<LikeRepository> logger)
            : base(db, logger)
        {
        }

        public Like Find(long userId, long pictureId)
        {
            return _db.Set<Like>()
                .Where(l => l.UserId.Equals(userId) 
                    && l.PictureId.Equals(pictureId))
                .FirstOrDefault();
        }

        public async Task<User> GetUser(long id)
        {
            var comment = await Get(id, "User");
            return comment.User;
        }
    }
}
