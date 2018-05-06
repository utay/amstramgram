using Core.Models;
using Microsoft.Extensions.Logging;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TagRepository : BaseRepository<Tag, long>, ITagRepository
    {
        public TagRepository() { }

        public TagRepository(AmstramgramContext db, ILogger<TagRepository> logger)
            : base(db, logger)
        {
        }

        public async Task<Picture> GetPicture(long id)
        {
            var tag = await Get(id, "Picture");
            return tag.Picture;
        }
    }
}
