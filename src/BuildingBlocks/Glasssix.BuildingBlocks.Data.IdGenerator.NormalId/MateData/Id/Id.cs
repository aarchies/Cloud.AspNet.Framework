using System;

namespace Glasssix.BuildingBlocks.Data.IdGenerator.NormalId.MateData.Id
{
    /// <summary>
    /// 标识生成器
    /// </summary>
    public static class Id
    {
        /// <summary>
        /// 标识
        /// </summary>
        private static string? _id;

        /// <summary>
        /// 获取Guid
        /// </summary>
        public static Guid GetGuid()
        {
            return string.IsNullOrWhiteSpace(_id) ? Guid.NewGuid() : _id.ToGuid();
        }

        public static long GetSnowflakeId()
        {
            return Snowflake.Instance.NextId();
        }

        public static string GetSnowflakeStringId()
        {
            return Snowflake.Instance.NextId().ToString();
        }

        /// <summary>
        /// 用Guid创建标识,去掉分隔符
        /// </summary>
        public static string GuidToString()
        {
            return string.IsNullOrWhiteSpace(_id) ? Guid.NewGuid().ToString("N") : _id;
        }

        /// <summary>
        /// 创建标识
        /// </summary>
        public static string ObjectId()
        {
            return string.IsNullOrWhiteSpace(_id) ? MateData.Id.ObjectId.GenerateNewStringId() : _id;
        }

        /// <summary>
        /// 重置标识
        /// </summary>
        public static void Reset()
        {
            _id = null;
        }

        /// <summary>
        /// 设置标识
        /// </summary>
        /// <param name="id">Id</param>
        public static void SetId(string id)
        {
            _id = id;
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj)
        {
            return Guid.Parse(obj);
        }
    }
}