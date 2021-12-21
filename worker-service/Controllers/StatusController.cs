using Shared;
using Microsoft.AspNetCore.Mvc;

namespace WorkerService.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStateManager stateManager;

        public StatusController(IStateManager jobStateManager)
        {
            stateManager = jobStateManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Post(string id)
        {
            var state = await stateManager.GetStateAsync(id);

            if (state == null) 
                return NotFound();

            return Ok(state);
        }               
    }
}
