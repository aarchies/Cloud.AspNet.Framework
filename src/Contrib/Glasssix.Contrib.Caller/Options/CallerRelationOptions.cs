using Glasssix.Contrib.Data.Options;
using System;

namespace Glasssix.Contrib.Caller.Options
{
    public class CallerRelationOptions : GlasssixRelationOptions<ICaller>
    {
        public CallerRelationOptions(string name, Func<IServiceProvider, ICaller> func)
            : base(name)
        {
            Func = func;
        }
    }
}