using Glasssix.Contrib.Domain.Shared.Pager;
using Glasssix.Utils.MetaEntitys.Lambdas;
using Glasssix.Utils.MetaEntitys.Objects;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Glasssix.Contrib.Repository.Queries
{
    /// <summary>
    /// 查询工具类
    /// </summary>
    public static class QueryHelper
    {
        /// <summary>
        /// 获取排序查询对象
        /// </summary>
        public static IOrderedQueryable<TEntity> GetOrderedQueryable<TEntity>(IQueryable<TEntity> source, IPager pager)
        {
            if (string.IsNullOrWhiteSpace(pager.Order))
                return source as IOrderedQueryable<TEntity>;
            return source.OrderBy(pager.Order);
        }

        /// <summary>
        /// 获取查询条件表达式
        /// </summary>
        /// <param name="predicate">查询条件,如果参数值为空，则忽略该查询条件，范例：t => t.Name == ""，该查询条件被忽略。
        /// 注意：一次仅能添加一个条件，范例：t => t.Name == "a" &amp;&amp; t.Mobile == "123"，不支持，将抛出异常</param>
        public static Expression<Func<TEntity, bool>> GetWhereIfNotEmptyExpression<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (predicate == null)
                return null;
            if (Lambda.GetConditionCount(predicate) > 1)
                throw new InvalidOperationException($"仅允许添加一个条件,条件：{predicate}");
            var value = predicate.Value();
            if (string.IsNullOrWhiteSpace(value.SafeString()))
                return null;
            return predicate;
        }

        /// <summary>
        /// 初始化排序
        /// </summary>
        public static void InitOrder<TEntity>(IQueryable<TEntity> source, IPager pager)
        {
            if (string.IsNullOrWhiteSpace(pager.Order) == false)
                return;
            if (source.Expression.SafeString().Contains(".OrderBy(") || source.Expression.SafeString().Contains(".OrderByDescending("))
                return;
            pager.Order = "Id";
        }
    }
}