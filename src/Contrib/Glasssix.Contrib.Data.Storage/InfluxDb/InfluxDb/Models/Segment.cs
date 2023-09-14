//using Glasssix.Contrib.Data.Storage.InfluxDb.Abstractions.Models;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Glasssix.Contrib.Data.Storage.InfluxDb.InfluxDb.Models
//{
//    [Measurement("segment")]
//    public class Segment : ISegment
//    {
//        [Column("tarce_id", IsTag = true)]
//        public string TraceId { get; set; }
//        [Column("trace_segment_id")]
//        public string TraceSegmentId { get; set; }
//        [Column("spans")]
//        public string Spans { get; set; }
//        [Column("service")]
//        public string Service { get; set; }
//        [Column("instance")]
//        public string ServiceInstance { get; set; }
//        [Column("is_size_limited")]
//        public bool IsSizeLimited { get; set; }
//        [Column(IsTimestamp = true)] public DateTime Time { get; set; } = DateTime.UtcNow;
//    }

//}