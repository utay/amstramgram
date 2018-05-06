using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository : BaseRepository<Comment, long>, ICommentRepository
    {
        public CommentRepository() { }

        public CommentRepository(AmstramgramContext db, ILogger<CommentRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<Picture> GetPicture(long id)
        {
            var comment = await Get(id, "Picture");
            return comment.Picture;
        }

        public async Task<User> GetUser(long id)
        {
            var comment = await Get(id, "User");
            return comment.User;
        }
    }
}
