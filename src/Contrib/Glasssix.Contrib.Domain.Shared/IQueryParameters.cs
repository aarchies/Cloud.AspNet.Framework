using Glasssix.Contrib.Domain.Shared.Pager;

namespace Glasssix.Contrib.Domain.Shared
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public interface IQueryParameters : IPager
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        string? Keywords { get; set; }
    }
}