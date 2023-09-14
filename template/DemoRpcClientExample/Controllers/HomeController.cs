using Glasssix.AspNetCore.Extension.Controller;
using Glasssix.AspNetCore.Rpc.Extension;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static Rpc.DemoService.RpcDemoService;

namespace DemoRpcClientExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> _logger;
        private RpcDemoServiceClient _Client { get; set; }
        private IServiceProxy _proxy;
        public HomeController(ILogger<HomeController> logger, RpcDemoServiceClient Client)
        {
            _logger = logger;
            _Client = Client;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _Client.GetListAsync(new Rpc.DemoService.Input() { Id = 1 });

            if (result.Success)
            {
                var str = Encoding.Default.GetString(result.Data.ToByteArray());
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                return Success(data);
            }
            return Success();
        }
    }
}