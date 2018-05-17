using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface ILikeRepository : IBaseRepository<Like, long>
    {
        Like Find(long userId, long pictureId);
        Task<User> GetUser(long id);
    }
}
