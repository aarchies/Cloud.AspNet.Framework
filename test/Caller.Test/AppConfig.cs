using Glasssix.BuildingBlocks.Configuration.Options;

namespace Glasssix.Contrib.Caller.DaprClient.Test
{
    public class AppConfig : LocalGlasssixConfigurationOptions
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Grpc端口
        /// </summary>
        public string GrpcEndpoint { get; set; }

        /// <summary>
        /// Http端口
        /// </summary>
        public string HttpEndpoint { get; set; }
    }
}