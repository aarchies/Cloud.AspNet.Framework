using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Elasticsearch.Model.LogModel
{
    public class LogResponseDto
    {
        public virtual Dictionary<string, object> Attributes { get; set; }
        public virtual object Body { get; set; }
        public virtual Dictionary<string, object> Resource { get; set; }
        public virtual int SeverityNumber { get; set; }
        public virtual string SeverityText { get; set; }
        public virtual string SpanId { get; set; }
        public virtual DateTime Timestamp { get; set; }

        public virtual int TraceFlags { get; set; }
        public virtual string TraceId { get; set; }
    }
}