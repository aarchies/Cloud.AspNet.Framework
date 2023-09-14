using Glasssix.Contrib.Message.Emqx;
using Microsoft.AspNetCore.Mvc;

namespace Emqx.SharedSubscribe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Glasssix.Contrib.Message.Emqx.IMqttClient _client;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMqttClient client)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public void Post(string groupname = "groupname", string topic = "topic")
        {
            //var sharedTopics = $"share/{groupname}/{topic}";
            var sharedTopics = $"{topic}";
            _client.PublishAsync(sharedTopics, $"当前订阅内容：GroupName:{groupname} Topic:{topic}");
            Console.WriteLine($"发布：{sharedTopics}");
        }

    }
}