namespace Glasssix.Contrib.Domain.Shared
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryParameters : Pager.Pager, IQueryParameters
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public virtual string? Keywords { get; set; }
    }
}