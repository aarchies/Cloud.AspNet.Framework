using Glasssix.Contrib.Data.Options;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Caller.Options
{
    public class CallerMiddlewareRelationOptions : GlasssixRelationOptions
    {
        public CallerMiddlewareRelationOptions(string name)
        {
            Name = name;
            Middlewares = new List<Func<IServiceProvider, ICallerMiddleware>>();
        }

        public List<Func<IServiceProvider, ICallerMiddleware>> Middlewares { get; }

        public void AddMiddlewareFunc(Func<IServiceProvider, ICallerMiddleware> func)
        {
            Middlewares.Add(func);
        }
    }
}