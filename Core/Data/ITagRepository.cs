using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface ITagRepository : IBaseRepository<Tag, long>
    {
        Task<Picture> GetPicture(long id);
    }
}
