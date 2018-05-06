using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    public interface IUserRepository : IBaseRepository<User, long>
    {
        Task<ICollection<Picture>> GetPictures(long id);
    }
}
