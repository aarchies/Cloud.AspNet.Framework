using System;

namespace Glasssix.Contrib.Data.Storage.Model.Trace
{
    public class TraceRequestAttrDto
    {
        public DateTime End { get; set; }
        public string Endpoint { get; set; }
        public string Instance { get; set; }
        public int MaxCount { get; set; }
        public string Query { get; set; }
        public string Service { get; set; }
        public DateTime Start { get; set; }
    }
}