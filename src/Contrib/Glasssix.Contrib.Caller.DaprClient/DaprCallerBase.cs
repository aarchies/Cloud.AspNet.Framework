using Dapr.Client;
using Glasssix.Contrib.Caller.DaprClient.Extensions;
using Glasssix.Contrib.Caller.DaprClient.Internal;
using System;

namespace Glasssix.Contrib.Caller.DaprClient
{
    public abstract class DaprCallerBase : CallerBase
    {
        protected DaprCallerBase()
        {
        }

        protected DaprCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public virtual Action<DaprClientBuilder> Configure { get; set; } = null;
        protected abstract string AppId { get; set; }

        public override void UseCallerExtension() => UseDapr();

        protected virtual void ConfigGlasssixCallerClient(GlasssixCallerClient callerClient)
        {
        }

        protected virtual GlasssixDaprClientBuilder UseDapr()
        {
            var daprClientBuilder = CallerOptions.UseDapr(callerClient =>
            {
                callerClient.AppId = AppId;
                ConfigGlasssixCallerClient(callerClient);
            }, Configure);
            daprClientBuilder.AddConfigHttpRequestMessage(ConfigHttpRequestMessageAsync);
            return daprClientBuilder;
        }
    }
}