namespace Glasssix.Contrib.Caller
{
    public static class Constant
    {
        public const string DEFAULT_APPID_DELIMITER = "-";

        public const string DEFAULT_ARGUMENT_PREFIX = "--";
        public const string DEFAULT_FILE_NAME = "dapr";

        /// <summary>
        /// 心跳检测间隔，用于检测dapr状态
        /// </summary>
        public const int DEFAULT_HEARTBEAT_INTERVAL = 5000;

        /// <summary>
        /// 默认重试次数
        /// </summary>
        public const int DEFAULT_RETRY_TIME = 10;
    }
}