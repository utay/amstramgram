using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IPictureRepository : IBaseRepository<Picture, long>
    {
        Task<User> GetUser(long id);
        Task<ICollection<Tag>> GetTags(long id);
        Task<ICollection<Comment>> GetComments(long id);
        Task<ICollection<Like>> GetLikes(long id);
    }
}
