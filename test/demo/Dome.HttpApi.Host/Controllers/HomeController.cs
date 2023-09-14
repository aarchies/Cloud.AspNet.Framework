using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dome.HttpApi.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SwaggerTag("ʾ������")]
    [Authorize(Roles = "admin")]
    public class HomeController
    {
    }
}