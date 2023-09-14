namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal.Enumerations
{
    internal enum CompressMode
    {
        /// <summary>
        /// 无压缩
        /// </summary>
        None = 1,

        /// <summary>
        /// 压缩但不反序列化
        /// </summary>
        Compress,

        /// <summary>
        /// 序列化和压缩
        /// </summary>
        SerializeAndCompress,
    }
}