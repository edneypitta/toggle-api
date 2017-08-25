using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toggle.Domain.Repositories;
using Entities = Toggle.Domain.Entities;

namespace Toggle.Infra.Repositories
{
    public class Toggles : IToggles
    {
        private readonly ToggleContext context;

        public Toggles(ToggleContext context) =>
            this.context = context;

        public async Task<Entities.Toggle> GetByIdAsync(int id) =>
            await context.Toggles.FindAsync(id);

        public IEnumerable<Entities.Toggle> GetGlobals() =>
            context.Toggles.Where(t => t.IsGlobal);

        public IEnumerable<Entities.Toggle> GetFromService(int serviceId, string version) =>
            context.Toggles.Where(t => t.BelongsTo(serviceId, version));

        public async Task CreateAsync(Entities.Toggle toggle)
        {
            context.Add(toggle);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Entities.Toggle toggle)
        {
            context.Update(toggle);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Entities.Toggle toggle)
        {
            context.Remove(toggle);
            await context.SaveChangesAsync();
        }
    }
}
