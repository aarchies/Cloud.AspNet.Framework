using Glasssix.Contrib.Caller.Options;
using Glasssix.Utils.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Caller.DaprClient
{
    public class DefaultCallerProvider : ICallerProvider
    {
        private readonly IConfiguration? _configuration;
        private readonly IOptionsMonitor<DaprOptions> _daprOptions;
        private readonly IGlasssixConfiguration? _GlasssixConfiguration;

        public DefaultCallerProvider(IOptionsMonitor<DaprOptions> daprOptions,
            IConfiguration? configuration = null,
            IGlasssixConfiguration? GlasssixConfiguration = null)
        {
            _daprOptions = daprOptions;
            _configuration = configuration;
            _GlasssixConfiguration = GlasssixConfiguration;
        }

        public string CompletionAppId(string appId)
        {
            var daprOptions = _daprOptions.CurrentValue;
            if (daprOptions.AppPort > 0 && daprOptions.IsIncompleteAppId())
                appId = $"{appId}{daprOptions.AppIdDelimiter}{daprOptions.AppIdSuffix}";

            var value = _configuration == null ? Environment.GetEnvironmentVariable(appId) : _configuration?.GetSection(appId).Value;
            if (string.IsNullOrWhiteSpace(value))
                value = _GlasssixConfiguration?.Local.GetSection(appId).Value;

            if (string.IsNullOrWhiteSpace(value)) return appId;

            return value;
        }
    }
}