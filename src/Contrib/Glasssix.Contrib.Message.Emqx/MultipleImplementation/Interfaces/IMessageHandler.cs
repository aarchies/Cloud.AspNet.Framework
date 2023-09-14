using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleAsync(MqttNetMessage message);
    }

    public class MqttNetMessage
    {
        public string ClientId { get; set; } = "";

        public string? Data { get; set; }

        public string Topic { get; set; } = "";
    }
}
