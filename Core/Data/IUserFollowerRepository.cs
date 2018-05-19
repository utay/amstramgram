using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IUserFollowerRepository : IBaseRepository<UserFollower, long>
    {
        UserFollower Find(long userId, long followerId);
        Task<User> GetUser(long id);
        Task<User> GetFollower(long id);
    }
}
