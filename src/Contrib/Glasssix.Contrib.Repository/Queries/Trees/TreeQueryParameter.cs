using Glasssix.Contrib.Domain.Shared;
using Glasssix.Utils.MetaEntitys.Objects;
using Glasssix.Utils.ReflectionConductor;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Glasssix.Contrib.Repository.Queries.Trees
{
    /// <summary>
    /// 树形查询参数
    /// </summary>
    public class TreeQueryParameter : TreeQueryParameter<string>, ITreeQueryParameter
    {
    }

    /// <summary>
    /// 树形查询参数
    /// </summary>
    public class TreeQueryParameter<TParentId> : QueryParameters, ITreeQueryParameter<TParentId>
    {
        private string _path = string.Empty;

        /// <summary>
        /// 初始化树形查询参数
        /// </summary>
        protected TreeQueryParameter()
        {
            Order = "SortId";
        }

        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "启用")]
        public virtual bool? Enabled { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int? Level { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        public virtual TParentId ParentId { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public virtual string Path
        {
            get => _path == null ? string.Empty : _path.Trim();
            set => _path = value;
        }

        /// <summary>
        /// 是否搜索
        /// </summary>
        public virtual bool IsSearch()
        {
            var items = Reflection.GetPublicProperties(this);
            return items.Any(t => IsSearchProperty(t.Text, t.Value));
        }

        /// <summary>
        /// 是否搜索属性
        /// </summary>
        protected virtual bool IsSearchProperty(string name, object value)
        {
            if (string.IsNullOrEmpty(value.SafeString()))
                return false;
            switch (name.SafeString().ToLower())
            {
                case "order":
                case "pagesize":
                case "page":
                case "totalcount":
                    return false;
            }
            return true;
        }
    }
}