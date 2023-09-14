using Dapper.Contrib.Extensions;
using Glasssix.BuildingBlocks.Domain.Implements;

namespace Demo.Domain.Model
{
    [Table("PersonSyncState")] //IPassivable //ICreationTime //IModificationTime
    public class Demo : AggregateRoot<Demo, long>
    {

        public Demo() : base(0L)
        {

        }

        /// <summary> 设备 </summary>
        public Demo(long id) : base(id)
        {

        }

        /// <summary>
        /// 人员Id
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 同步状态
        /// </summary>
        public bool Status { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        public string? Msg { get; set; }


        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
