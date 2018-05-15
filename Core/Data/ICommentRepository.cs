using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface ICommentRepository : IBaseRepository<Comment, long>
    {
        Task<Picture> GetPicture(long id);
        Task<User> GetUser(long id);
    }
}
