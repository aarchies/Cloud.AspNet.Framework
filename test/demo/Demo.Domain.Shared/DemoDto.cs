using Glasssix.Contrib.Domain.Shared.Dtos;

namespace Demo.Domain.Shared
{
    public class DemoDto : DtoBase
    {
        public DemoDto()
        { }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 设备Id
        /// </summary>
        public string? DeviceId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 人员Id
        /// </summary>
        public string? PersonId { get; set; } = string.Empty;

        /// <summary>
        /// 同步状态
        /// </summary>
        public bool Status { get; set; }
    }
}