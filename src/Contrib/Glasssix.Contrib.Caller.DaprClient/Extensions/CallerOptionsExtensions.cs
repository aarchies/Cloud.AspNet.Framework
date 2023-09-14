using Dapr.Client;
using Glasssix.Contrib.Caller.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
namespace Glasssix.Contrib.Caller.DaprClient.Extensions
{
    public static class CallerOptionsExtensions
    {
        public static GlasssixDaprClientBuilder UseDapr(this CallerOptionsBuilder callerOptionsBuilder,
            Action<GlasssixDaprClient> masDaprClientConfigure, Action<DaprClientBuilder> configure = null)
        {

            callerOptionsBuilder.Services.AddDaprClient(configure);

            return callerOptionsBuilder.UseDaprCore(() =>
            {
                callerOptionsBuilder.Services.AddCaller(callerOptionsBuilder.Name, serviceProvider =>
                {
                    var GlasssixDaprClient = new GlasssixDaprClient();
                    masDaprClientConfigure.Invoke(GlasssixDaprClient);
                    var appid = serviceProvider.GetRequiredService<ICallerProvider>().CompletionAppId(GlasssixDaprClient.AppId);

                    var service = new DaprCaller(
                        serviceProvider,
                        callerOptionsBuilder.Name,
                        appid,
                        GlasssixDaprClient.RequestMessageFactory,
                        GlasssixDaprClient.ResponseMessageFactory);
                    return service;
                });
            });
        }

        private static GlasssixDaprClientBuilder UseDaprCore(this CallerOptionsBuilder callerOptionsBuilder, Action action)
        {
            callerOptionsBuilder.Services.TryAddSingleton<ICallerProvider, DefaultCallerProvider>();
            callerOptionsBuilder.Services.AddOptions();
            action.Invoke();
            return new GlasssixDaprClientBuilder(callerOptionsBuilder.Services, callerOptionsBuilder.Name);
        }
    }
}