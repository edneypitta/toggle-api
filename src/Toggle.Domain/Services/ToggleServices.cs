using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toggle.Domain.Exceptions;
using Toggle.Domain.Repositories;

namespace Toggle.Domain.Services
{
    public class ToggleServices
    {
        private readonly IServices services;
        private readonly IToggles toggles;

        public ToggleServices(IServices services, IToggles toggles)
        {
            this.services = services;
            this.toggles = toggles;
        }

        public IEnumerable<Entities.Toggle> GetFromService(int serviceId, string version)
        {
            var globalToggles = toggles.GetGlobals();
            var serviceToggles = toggles.GetFromService(serviceId, version);

            return serviceToggles.Union(globalToggles.Where(g => serviceToggles.All(s => s.Name != g.Name)));
        }

        public async Task<Entities.Toggle> GetById(int id) =>
            await toggles.GetByIdAsync(id);

        public async Task<Entities.Toggle> CreateAsync(int serviceId, string version, string name, string value)
        {
            var service = await services.GetByIdAsync(serviceId);
            if (service == null)
                throw new ServiceNotFoundException();

            var toggle = new Entities.Toggle(serviceId, version, name, value);
            await toggles.CreateAsync(toggle);

            return toggle;
        }

        public async Task UpdateAsync(int id, string name, string value)
        {
            var toggle = await toggles.GetByIdAsync(id);
            if (toggle == null)
                throw new ToggleNotFoundException();

            toggle.Update(name, value);

            await toggles.UpdateAsync(toggle);
        }

        public async Task DeleteAsync(int id)
        {
            var toggle = await toggles.GetByIdAsync(id);
            if (toggle == null)
                throw new ToggleNotFoundException();

            await toggles.DeleteAsync(toggle);
        }
    }
}
