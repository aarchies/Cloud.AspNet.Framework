using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Apm.Api.Test
{
    public static class DiagnosticsConfig
    {
        public static string ServiceName = AppDomain.CurrentDomain.FriendlyName!;
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
        public static Meter Meter = new(ServiceName);
        public static Counter<long> RequestCounter = Meter.CreateCounter<long>("app.request_counter");
    }
}
