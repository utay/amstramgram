using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IUserFollowerRepository : IBaseRepository<UserFollower, long>
    {
        Task<UserFollower> Find(long userId, long followerId);
        void Delete(long userId, long followerId);
    }
}
