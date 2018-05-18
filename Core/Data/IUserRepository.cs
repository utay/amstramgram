using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IUserRepository : IBaseRepository<User, long>
    {
        Task<ICollection<Picture>> GetPictures(long id);

        Task<ICollection<UserFollower>> GetFollowers(long id);
        Task<ICollection<UserFollower>> GetFollowing(long id);

        Task<User> GetFromAccessToken(string accessToken);

        Task<User> GetFromEmail(string email);
    }
}
