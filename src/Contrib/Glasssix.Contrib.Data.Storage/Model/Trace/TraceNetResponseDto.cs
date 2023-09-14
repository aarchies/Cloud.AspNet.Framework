namespace Glasssix.Contrib.Data.Storage.Model.Trace
{
    public class TraceNetResponseDto
    {
        public virtual string CarrierICC { get; set; }
        public virtual string CarrierMCC { get; set; }
        public virtual string CarrierMNC { get; set; }
        public virtual string CarrierName { get; set; }
        public virtual string HostConnectSubtype { get; set; }
        public virtual string HostConnectType { get; set; }
        public virtual string HostIp { get; set; }
        public virtual string HostName { get; set; }
        public virtual int HostPort { get; set; }
        public virtual string PeerIp { get; set; }
        public virtual string PeerName { get; set; }
        public virtual int PeerPort { get; set; }
        public virtual string PeerService { get; set; }
        public virtual string Transport { get; set; }
    }
}