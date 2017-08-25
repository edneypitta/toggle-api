using System.Collections.Generic;
using System.Threading.Tasks;

namespace Toggle.Domain.Repositories
{
    public interface IToggles
    {
        Task<Entities.Toggle> GetByIdAsync(int id);
        IEnumerable<Entities.Toggle> GetGlobals();
        IEnumerable<Entities.Toggle> GetFromService(int serviceId, string version);
        Task CreateAsync(Entities.Toggle toggle);
        Task UpdateAsync(Entities.Toggle toggle);
        Task DeleteAsync(Entities.Toggle toggle);
    }
}