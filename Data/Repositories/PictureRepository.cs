using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PictureRepository : BaseRepository<Picture, long>, IPictureRepository
    {
        public PictureRepository() { }

        public PictureRepository(AmstramgramContext db, ILogger<PictureRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<ICollection<Comment>> GetComments(long id)
        {
            var picture = await Get(id, "Comments");
            return picture.Comments;
        }

        public async Task<ICollection<Like>> GetLikes(long id)
        {
            var picture = await Get(id, "Likes");
            return picture.Likes;
        }

        public async Task<ICollection<Tag>> GetTags(long id)
        {
            var picture = await Get(id, "Tags");
            return picture.Tags;
        }

        public async Task<User> GetUser(long id)
        {
            var picture = await Get(id, "User");
            return picture.User;
        }
    }
}
