using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface ILikeRepository : IBaseRepository<Like, long>
    {
        Task<User> GetUser(long id);
        Task<Picture> GetPicture(long id);
        Task<Like> Find(long userId, long pictureId);
    }
}
