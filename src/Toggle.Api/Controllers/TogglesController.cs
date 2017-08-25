using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toggle.Api.ViewModels;
using Toggle.Domain.Exceptions;
using Toggle.Domain.Services;
using Entities = Toggle.Domain.Entities;

namespace Toggle.Api.Controllers
{
    [Route("api/[controller]")]
    public class TogglesController : Controller
    {
        private readonly ToggleServices toggleServices;

        public TogglesController(ToggleServices toggleServices) =>
            this.toggleServices = toggleServices;

        [HttpGet("{serviceId}/{version}")]
        public IEnumerable<Entities.Toggle> Get(int serviceId, string version) =>
            toggleServices.GetFromService(serviceId, version);


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var toggle = await toggleServices.GetById(id);
            if (toggle == null)
                return NotFound();

            return new ObjectResult(toggle);
        }

        [HttpPost("{serviceId}/{version}")]
        public async Task<IActionResult> PostAsync(int serviceId, string version, [FromBody]ToggleViewModel viewModel)
        {
            try
            {
                var toggle = await toggleServices.CreateAsync(serviceId, version, viewModel.Name, viewModel.Value);
                return Created($"/api/toggles/{toggle.Id}", toggle);
            }
            catch (ServiceNotFoundException)
            {
                return NotFound($"Service {serviceId} does not exist");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]ToggleViewModel viewModel)
        {
            try
            {
                await toggleServices.UpdateAsync(id, viewModel.Name, viewModel.Value);
                return new NoContentResult();
            }
            catch (ToggleNotFoundException)
            {
                return NotFound($"Toggle {id} does not exist");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await toggleServices.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (ToggleNotFoundException)
            {
                return NotFound($"Toggle {id} does not exist");
            }
        }
    }
}
