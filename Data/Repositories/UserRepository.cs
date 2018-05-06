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
    }
}
