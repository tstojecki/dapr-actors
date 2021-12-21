using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly HttpClient invokeHttpClient;

        public StatusController()
        {
            invokeHttpClient = DaprClient.CreateInvokeHttpClient("workerservice");
        }

        //[HttpGet("{id}")]
        //public IActionResult Get(string id)
        //{
        //    await return invokeHttpClient.GetAsync($"status/{id}");
        //}
    }
}
