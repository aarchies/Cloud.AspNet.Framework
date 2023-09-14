using Glasssix.Contrib.Data.Options;
using System;
using System.Linq;

namespace Glasssix.Contrib.Caller.Options
{
    public class CallerMiddlewareFactoryOptions : GlasssixFactoryOptions<CallerMiddlewareRelationOptions>
    {
        public void AddMiddleware(string name, Func<IServiceProvider, ICallerMiddleware> implementationFactory)
        {
            var option = Options.FirstOrDefault(o => o.Name!.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (option != null) option.AddMiddlewareFunc(implementationFactory);
            else Options.Add(new CallerMiddlewareRelationOptions(name)
            {
                Middlewares = { implementationFactory }
            });
        }
    }
}