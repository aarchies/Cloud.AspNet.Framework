using Glasssix.Contrib.Domain.Shared.Pager;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Glasssix.Contrib.Services.Extensions
{
    internal static class ExpressionExtensions
    {
        internal static BinaryExpression CompareExpression(this Compare compare, Expression left, Expression right)
        {
            switch (compare)
            {
                case Compare.NotEqual:
                    return Expression.NotEqual(left, right);

                case Compare.LessThan:
                    return Expression.LessThan(left, right);

                case Compare.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);

                case Compare.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);

                case Compare.GreaterThan:
                    return Expression.GreaterThan(left, right);

                default:
                    return Expression.Equal(left, right);
            }
        }

        internal static IEnumerable<PropertyInfo> Exclude(this List<PropertyInfo> properties)
        {
            var pageProperties = typeof(Pager).GetProperties().Select(x => x.Name);

            var result = properties.Where(x => !pageProperties.Contains(x.Name));
            return result;
        }
    }
}