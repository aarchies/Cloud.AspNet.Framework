using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Orm.SqlSugar
{
    public class ClientBase
    {
        private readonly ISdkConfig _sdkConfig;

        public ClientBase(ISdkConfig config)
        {
            _sdkConfig = config;
        }

        /// <summary>
        /// 通用GET请求
        /// </summary>
        /// <returns></returns>
        protected async Task<TResult?> GetAsync<TResult>(string requestUrl) where TResult : class
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(result))
                        {
                            TResult t = result.JsonToEntity<TResult>();

                            return t;
                        }
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 常规检查
        /// </summary>
        protected void NormalizeChecked()
        {
            //检查是否启用SDK
            if (!_sdkConfig.Enabled) throw new Exception("当前SDK配置为未启用");
        }
    }
}