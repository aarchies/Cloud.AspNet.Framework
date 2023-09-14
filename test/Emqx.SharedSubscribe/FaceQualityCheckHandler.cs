using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Interfaces;

namespace Emqx.SharedSubscribe
{
    /// <summary>
    /// 人脸入库质量判断回传结果
    /// </summary>
    [Message("server/auth/run", "A")]
    public class FaceQualityCheckHandler : MessageHandlerBase
    {
        public override Task HandleAsync(MqttNetMessage message)
        {
            Console.WriteLine($"收到消息：{message.Data} Topic:{message.Topic}");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 人脸入库质量判断回传结果
    /// </summary>
    [Message("Server/Auth/Run/OJASDa", "A")]
    public class FaceQualityCheckHandlers : MessageHandlerBase
    {
        public override Task HandleAsync(MqttNetMessage message)
        {
            Console.WriteLine($"收到消息s：{message.Data} Topic:{message.Topic}");
            return Task.CompletedTask;
        }
    }
}
