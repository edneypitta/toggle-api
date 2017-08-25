using System.Threading.Tasks;
using Toggle.Domain.Entities;

namespace Toggle.Domain.Repositories
{
    public interface IServices
    {
        Task<Service> GetByIdAsync(int id);
    }
}