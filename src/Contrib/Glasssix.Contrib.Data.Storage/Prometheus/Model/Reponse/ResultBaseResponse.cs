using Glasssix.Contrib.Data.Storage.Prometheus.Enums;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse
{
    public class ResultBaseResponse
    {
        public string? Error { get; set; }
        public string? ErrorType { get; set; }
        public ResultStatuses Status { get; set; }
        public IEnumerable<string>? Warnings { get; set; }
    }
}