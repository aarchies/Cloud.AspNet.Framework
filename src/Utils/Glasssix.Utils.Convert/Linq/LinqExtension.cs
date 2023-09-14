using System;
using System.Linq;
using System.Linq.Expressions;

namespace Glasssix.Utils.Convert.Linq
{
    public static class LinqExtension
    {
        public static IQueryable<TEntity> WhereIF<TEntity>
            (this IQueryable<TEntity> source, bool comb, Expression<Func<TEntity, bool>> predicate)
        {
            if (comb)
                source = source.Where(predicate);
            return source;
        }
    }
}