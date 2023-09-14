using System.Threading.Tasks;

namespace Glasssix.Contrib.Data
{
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// ����ConnectionName��ȡ���ݿ������ַ���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConnectionString(string name = ConnectionStrings.DEFAULT_CONNECTION_STRING_NAME);

        /// <summary>
        /// ����ConnectionName��ȡ���ݿ������ַ���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> GetConnectionStringAsync(string name = ConnectionStrings.DEFAULT_CONNECTION_STRING_NAME);
    }
}