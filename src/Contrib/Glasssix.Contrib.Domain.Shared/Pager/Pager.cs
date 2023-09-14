namespace Glasssix.Contrib.Domain.Shared.Pager
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class Pager : IPager
    {
        protected int _pageIndex;

        /// <summary>
        /// 初始化分页参数
        /// </summary>
        public Pager()
            : this(1)
        {
        }

        /// <summary>
        /// 初始化分页参数
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数,默认20</param>
        /// <param name="order">排序条件</param>
        public Pager(int page, int pageSize, string order)
            : this(page, pageSize, 0, order)
        {
        }

        /// <summary>
        /// 初始化分页参数
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数,默认20</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="order">排序条件</param>
        public Pager(int page, int pageSize = 20, int totalCount = 0, string order = "")
        {
            Page = page;
            PageSize = pageSize;
            Total = totalCount;
            Order = order;
        }

        /// <summary>
        /// 排序条件
        /// </summary>
        public virtual string Order { get; set; }

        /// <summary>
        /// 页索引，即第几页，从1开始
        /// </summary>
        public virtual int Page
        {
            get => GetPageIndex();
            set => _pageIndex = value;
        }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public virtual int Total { get; set; }

        /// <summary>
        /// 结束行数
        /// </summary>
        public int GetEndNumber()
        {
            return Page * PageSize;
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int GetPageCount()
        {
            if (Total % PageSize == 0)
                return Total / PageSize;
            return Total / PageSize + 1;
        }

        /// <summary>
        /// 获取跳过的行数
        /// </summary>
        public int GetSkipCount()
        {
            return PageSize * (Page - 1);
        }

        /// <summary>
        /// 起始行数
        /// </summary>
        public int GetStartNumber()
        {
            return (Page - 1) * PageSize + 1;
        }

        /// <summary>
        /// 获取页码
        /// </summary>
        /// <returns></returns>
        protected int GetPageIndex()
        {
            if (_pageIndex <= 0)
                _pageIndex = 1;
            return _pageIndex;
        }
    }
}