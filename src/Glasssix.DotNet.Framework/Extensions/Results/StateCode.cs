namespace Glasssix.DotNet.Framework.Extensions.Results
{
    public enum HealthStateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 失败 请求异常，errorMsg包含提示信息
        /// </summary>
        Abnormal = -1,

        /// <summary>
        /// 请求超时
        /// </summary>
        TimeOut = -2,

        /// <summary>
        /// 令牌异常
        /// </summary>
        TokenAbnormal = -3,

        /// <summary>
        /// 签名异常
        /// </summary>
        SignAbnormal = -4,

        /// <summary>
        /// 请求频率超出限制
        /// </summary>
        LimitAbnormal = -5,

        /// <summary>
        /// 未请求到防疫健康核验信息
        /// </summary>
        NotFoundHealth = -6,

        /// <summary>
        /// 密钥不匹配
        /// </summary>
        AppIdAbnormal = -7,

        /// <summary>
        /// 请求被限制
        /// </summary>
        RequestLimit = -8,
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2
    }
}