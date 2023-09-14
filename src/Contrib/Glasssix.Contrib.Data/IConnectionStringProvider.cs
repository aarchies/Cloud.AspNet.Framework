using System.Threading.Tasks;

namespace Glasssix.Contrib.Data
{
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// 根据ConnectionName获取数据库连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConnectionString(string name = ConnectionStrings.DEFAULT_CONNECTION_STRING_NAME);

        /// <summary>
        /// 根据ConnectionName获取数据库连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> GetConnectionStringAsync(string name = ConnectionStrings.DEFAULT_CONNECTION_STRING_NAME);
    }
}