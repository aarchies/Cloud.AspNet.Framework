using MQTTnet.Client;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation
{
    public interface IMessageQueueClient
    {
        MQTTnet.Client.IMqttClient Connnection { get; }

        Task<MqttClientPublishResult> PublishAsync(string key, string body);

        Task ReConnnectionAsync();
    }
}
