using FSPBook.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories
{
    public interface IProfileRepository
    {
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile> GetProfileByIdAsync(int id);
    }
}
