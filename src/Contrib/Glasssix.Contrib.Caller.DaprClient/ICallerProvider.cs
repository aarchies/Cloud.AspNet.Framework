namespace Glasssix.Contrib.Caller.DaprClient
{
    public interface ICallerProvider
    {
        /// <summary>
        /// 根据服务id获得的dapr-appid
        /// 当配置中不存在指定服务的dapr-appid时，将服务id作为dapr-appd返回
        /// </summary>
        /// <param name="appId">service appid</param>
        /// <returns></returns>
        string CompletionAppId(string appId);
    }
}