using Glasssix.Contrib.Message.Emqx.Meta;
using System;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx
{
    public interface IMqttClient
    {
        event Action<MqttNetMessage> ReciveMsg;

        /// <summary>
        /// 获取客户端实例
        /// </summary>
        /// <returns></returns>
        Task<MQTTnet.Client.IMqttClient> GetMqttClientInterface();

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<bool> PublishAsync(string topic, string body);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task Subscribe(string topic);
    }
}