using Microsoft.Extensions.Options;

namespace Glasssix.Contrib.Message.Emqx.Option
{
    public class MqttOptions : IOptions<MqttOptions>
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// 证书文件
        /// </summary>
        public string? Crtfile { get; set; }

        /// <summary>
        /// 证书密钥
        /// </summary>
        public string? Keyfile { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Api地址
        /// </summary>
        public string? ApiClient { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Vsrsion { get; set; }

        public MqttOptions Value => this;
    }
}