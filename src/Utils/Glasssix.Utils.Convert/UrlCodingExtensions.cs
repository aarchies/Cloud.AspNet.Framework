namespace Glasssix.Utils.Convert
{
    public static class UrlCodingExtensions
    {
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }
    }
}