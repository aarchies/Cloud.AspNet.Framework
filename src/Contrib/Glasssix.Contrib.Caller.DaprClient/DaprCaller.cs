using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller.DaprClient
{
    public class DaprCaller : AbstractCaller
    {
        protected readonly string AppId;
        private Dapr.Client.DaprClient? _daprClient;

        public DaprCaller(
            IServiceProvider serviceProvider,
            string name,
            string appId,
            Func<IServiceProvider, IRequestMessage>? requestMessageFactory,
            Func<IServiceProvider, IResponseMessage>? responseMessageFactory)
            : base(serviceProvider, name, requestMessageFactory, responseMessageFactory)
        {
            AppId = appId;
            var logger = serviceProvider.GetService<ILogger<DaprCaller>>();
            logger?.LogDebug("The Name of the initialized Caller is {Name}, and the AppId is {AppId}", name, appId);
        }

        private Dapr.Client.DaprClient DaprClient => _daprClient ??= ServiceProvider.GetRequiredService<Dapr.Client.DaprClient>();

        [ExcludeFromCodeCoverage]
        public override HttpRequestMessage CreateRequest(HttpMethod method, string? methodName)
        {
            var requestMessage = DaprClient.CreateInvokeMethodRequest(method, AppId, methodName);
            RequestMessage.ProcessHttpRequestMessage(requestMessage);
            return requestMessage;
        }

        [ExcludeFromCodeCoverage]
        public override HttpRequestMessage CreateRequest<TRequest>(HttpMethod method, string? methodName, TRequest data)
        {
            var requestMessage = DaprClient.CreateInvokeMethodRequest(method, AppId, methodName);
            RequestMessage.ProcessHttpRequestMessage(requestMessage, data);
            return requestMessage;
        }

        [ExcludeFromCodeCoverage]
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
            => await DaprClient.InvokeMethodWithResponseAsync(request, cancellationToken);

        [ExcludeFromCodeCoverage]
        public override Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default)
            => DaprClient.InvokeMethodGrpcAsync(AppId, methodName, cancellationToken);

        [ExcludeFromCodeCoverage]
        public override Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
            => DaprClient.InvokeMethodGrpcAsync<TResponse>(AppId, methodName, cancellationToken);

        [ExcludeFromCodeCoverage]
        public override Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
            => DaprClient.InvokeMethodGrpcAsync(AppId, methodName, request, cancellationToken);

        [ExcludeFromCodeCoverage]
        public override Task<TResponse> SendGrpcAsync<TRequest, TResponse>(
            string methodName,
            TRequest request,
            CancellationToken cancellationToken = default)
            => DaprClient.InvokeMethodGrpcAsync<TRequest, TResponse>(AppId, methodName, request, cancellationToken);
    }
}