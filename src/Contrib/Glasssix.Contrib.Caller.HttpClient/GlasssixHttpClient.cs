using System;

namespace Glasssix.Contrib.Caller.HttpClient
{
    public class GlasssixHttpClient : GlasssixCallerClient
    {
        public string? BaseAddress { get; set; }
        public string BaseApi { get => BaseAddress!; set => BaseAddress = value; }
        public Action<System.Net.Http.HttpClient>? Configure { get; set; }
        public string? Prefix { get; set; }

        internal void ConfigureHttpClient(System.Net.Http.HttpClient httpClient)
        {
            if (!string.IsNullOrEmpty(BaseAddress))
                httpClient.BaseAddress = new Uri(BaseAddress);

            Configure?.Invoke(httpClient);
        }
    }
}