using Demo.Application.Contracts;
using Demo.Domain.Shared;
using Glasssix.DotNet.Framework.Extensions.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dome.HttpApi.Host.Controllers
{
    [ApiController]
    [SwaggerTag("ʾ������")]
    [Authorize]
    public class HomeController : SampleController<DemoDto, DemoInput>
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDemoService _service;

        public HomeController(ILogger<HomeController> logger, IDemoService service) : base(service)
        {
            _logger = logger;
            _service = service;

        }

        /// <summary>
        /// Get��д
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<IActionResult> GetAsync(string id)
        {
            var a = this.GetRoleName();
            var s = this.GetUserName();
            var c = this.GetToken();
            //  var result = await _redis.ObjectGetAsync<DemoMapDto>(id);
            //if (result == null)
            //{
            //    result = (await _service.GetByIdAsync(id)).MapTo<DemoMapDto>();
            //    //await _redis.ObjectSetAsync<DemoMapDto>(id, result);
            //}
            return Success();
        }
    }
}