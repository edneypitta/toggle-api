using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toggle.Api.ViewModels;
using Toggle.Domain.Exceptions;
using Toggle.Domain.Services;
using Entities = Toggle.Domain.Entities;

namespace Toggle.Api.Controllers
{
    [Route("api/[controller]/{serviceId}/{version}")]
    public class TogglesController : Controller
    {
        private readonly ToggleServices toggleServices;

        public TogglesController(ToggleServices toggleServices) =>
            this.toggleServices = toggleServices;

        [HttpGet]
        public IEnumerable<Entities.Toggle> Get(int serviceId, string version) =>
            toggleServices.GetFromService(serviceId, version);

        [HttpPost]
        public async Task<ActionResult> PostAsync(int serviceId, string version, [FromBody]ToggleViewModel viewModel)
        {
            try
            {
                await toggleServices.CreateAsync(serviceId, version, viewModel.Name, viewModel.Value);
            }
            catch (ServiceNotFoundException)
            {
                return BadRequest($"Service {serviceId} does not exist");
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody]ToggleViewModel viewModel)
        {
            try
            {
                await toggleServices.UpdateAsync(id, viewModel.Name, viewModel.Value);
            }
            catch (ToggleNotFoundException)
            {
                return BadRequest($"Toggle {id} does not exist");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await toggleServices.DeleteAsync(id);
            }
            catch (ToggleNotFoundException)
            {
                return BadRequest($"Toggle {id} does not exist");
            }

            return Ok();
        }
    }
}
