namespace Glasssix.DotNet.Framework.Extensions.Filters.Authentication
{
    public class TokeResponse
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 生命周期
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 作用域
        /// </summary>
        public string scope { get; set; }

        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; }
    }
}