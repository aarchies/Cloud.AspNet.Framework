using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Model
{
    public class BaseRequestDto : RequestPageBase
    {
        public IEnumerable<FieldConditionDto> Conditions { get; set; }
        public DateTime End { get; set; }
        public string Endpoint { get; set; }
        public string Instance { get; set; }
        public string Keyword { get; set; }
        public string RawQuery { get; set; }
        public string Service { get; set; }
        public FieldOrderDto? Sort { get; set; }
        public DateTime Start { get; set; }
        public string TraceId { get; set; }

        public virtual void AppendConditions()
        { }
    }
}