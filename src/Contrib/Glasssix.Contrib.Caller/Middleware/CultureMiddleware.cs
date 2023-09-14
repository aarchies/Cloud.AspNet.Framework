using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller.Middleware
{
    internal class CultureMiddleware : ICallerMiddleware
    {
        public Task HandleAsync(GlasssixHttpContext GlasssixHttpContext, CallerHandlerDelegate next, CancellationToken cancellationToken = default)
        {
            var name = "cookie";
            if (GlasssixHttpContext.RequestMessage.Headers.TryGetValues(name, out IEnumerable<string>? cookieValues))
            {
                if (IsExistCulture(cookieValues)) return next();

                GlasssixHttpContext.RequestMessage.Headers.Remove(name);
            }

            var cookies = cookieValues?.ToList() ?? new List<string>();
            cookies.Add($".AspNetCore.Culture={GetCultureValue()}");
            GlasssixHttpContext.RequestMessage.Headers.Add(name, cookies);
            return next();
        }

        private static string GetCultureValue() => GetCultureValue(new (string Key, string Value)[]
        {
        ("c", CultureInfo.CurrentCulture.Name),
        ("uic", CultureInfo.CurrentUICulture.Name)
        });

        private static string GetCultureValue((string Key, string Value)[] cultures)
            => System.Web.HttpUtility.UrlEncode(string.Join('|', cultures.Select(c => $"{c.Key}={c.Value}")));

        private static bool IsExistCulture(IEnumerable<string>? cookieValues)
                    => cookieValues != null && cookieValues.Any(cookie => cookie.Contains(".AspNetCore.Culture=", StringComparison.OrdinalIgnoreCase));
    }
}