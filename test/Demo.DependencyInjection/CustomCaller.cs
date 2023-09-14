using Glasssix.Contrib.Caller.HttpClient;

namespace Glasssix.Contrib.Caller.HttprClient.Test
{
    public class CustomCaller : HttpClientCallerBase
    {
        protected override string BaseAddress { get; set; } = "http://localhost:5000";

        public Task<string?> HelloAsync(string name) => Caller.GetAsync<string>($"/Hello", new { Name = name });
    }
}