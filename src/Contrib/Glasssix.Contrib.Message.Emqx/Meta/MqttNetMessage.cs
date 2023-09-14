namespace Glasssix.Contrib.Message.Emqx.Meta
{
    public class MqttNetMessage
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string? Topic { get; set; }
    }
}