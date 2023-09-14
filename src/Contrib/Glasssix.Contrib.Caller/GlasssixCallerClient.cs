using System;

namespace Glasssix.Contrib.Caller
{
    public class GlasssixCallerClient
    {
        public Func<IServiceProvider, IRequestMessage>? RequestMessageFactory { get; set; } = null;

        public Func<IServiceProvider, IResponseMessage>? ResponseMessageFactory { get; set; } = null;
    }
}