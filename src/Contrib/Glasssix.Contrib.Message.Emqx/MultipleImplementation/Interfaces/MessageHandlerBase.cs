using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation.Interfaces
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        public abstract Task HandleAsync(MqttNetMessage message);
    }
}
