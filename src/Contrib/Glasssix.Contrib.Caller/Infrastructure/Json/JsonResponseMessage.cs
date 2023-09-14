using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller.Infrastructure.Json
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class JsonResponseMessage : DefaultResponseMessage
    {
        private readonly JsonSerializerOptions? _jsonSerializerOptions;

        public JsonResponseMessage(
            JsonSerializerOptions? jsonSerializerOptions = null,
            ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<DefaultResponseMessage>())
        {
            _jsonSerializerOptions = jsonSerializerOptions ?? GlasssixIocApp.GetJsonSerializerOptions();
        }

        protected override Task<TResponse?> FormatResponseAsync<TResponse>(HttpContent httpContent,
            CancellationToken cancellationToken = default) where TResponse : default
        {
            try
            {
                return httpContent.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions, cancellationToken);
            }
            catch (Exception exception)
            {
                Logger?.LogWarning(exception, "{Message}", exception.Message);
                ExceptionDispatchInfo.Capture(exception).Throw();
                return default; //This will never be executed, the previous line has already thrown an exception
            }
        }
    }
}