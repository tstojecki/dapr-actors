using Dapr;
using Shared;
using Microsoft.AspNetCore.Mvc;

namespace WorkerService.Controllers
{
    public class LongRunningController : Controller
    {
        private readonly IStateManager stateManager;
        private readonly ILogger<LongRunningController> logger;

        public LongRunningController(IStateManager jobStateManager, ILogger<LongRunningController> logger)
        {
            stateManager = jobStateManager;

            this.logger = logger;
        }

        [HttpPost("longrunningjob")]
        [Topic("pubsub", "longrunningjob")]
        public async Task<IActionResult> Post([FromBody] LongRunningRequest request)
        {
            var state = await stateManager.GetStateAsync(request.Id);
            if (state != null)
            {
                return BadRequest("request is already being handled");
            }

            state = new State
            {
                Id = request.Id,
                Created = DateTime.Now
            };

            try
            {
                for (int i = 0; i < request.Iterations; i++)
                {
                    logger.LogInformation("{i}: {message}", i, request.Message);

                    state.Counter++;

                    if (request.ErrorAtIteration > 0 && request.ErrorAtIteration == i)
                    {
                        throw new Exception($"Error running request {request.Id}");
                    }

                    logger.LogInformation("Setting state using state manager of type {type}", stateManager.GetType().Name);
                    await stateManager.SetStateAsync(state);

                    logger.LogInformation("Delaying for {delay}ms", request.Delay);
                    await Task.Delay(request.Delay);
                }                
            }
            finally
            {
                await stateManager.RemoveStateAsync(request.Id);
            }

            return Ok();
        }               
    }
    public class LongRunningRequest
    {
        public string Id { get; set; }
        public int Iterations { get; set; }
        public int ErrorAtIteration { get; set; }
        public int Delay { get; set; }
        public string Message { get; set; }
    }
}
