using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IUserFollowerRepository : IBaseRepository<UserFollower, long>
    {
        Task<User> GetUser(long id);
        Task<User> GetFollower(long id);
        Task<Like> Find(long userId, long followerId);
    }
}
