using System.Threading.Tasks;
using Toggle.Domain.Entities;
using Toggle.Domain.Repositories;

namespace Toggle.Infra.Repositories
{
    public class Services : IServices
    {
        private readonly ToggleContext context;

        public Services(ToggleContext context) =>
            this.context = context;

        public async Task<Service> GetByIdAsync(int id) =>
            await context.Services.FindAsync(id);
    }
}
