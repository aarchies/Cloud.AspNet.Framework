using Microsoft.AspNetCore.Mvc;

namespace Apm.Api.Test.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IActionResult Get()
        {
            // Track work inside of the request
            using var activity = DiagnosticsConfig.ActivitySource.StartActivity("SayHello");
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });

            DiagnosticsConfig.RequestCounter.Add(1,
                new("Action", nameof(Index)),
                new("Controller", nameof(HomeController)));
            return NoContent();
        }

        [HttpGet(Name = "Get1")]
        public IActionResult Get1()
        {
            DiagnosticsConfig.RequestCounter.Add(1,
                new("Action", nameof(Index)),
                new("Controller", nameof(HomeController)));
            return NoContent();
        }
    }
}