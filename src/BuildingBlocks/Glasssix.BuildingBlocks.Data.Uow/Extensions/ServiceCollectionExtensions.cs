using Glasssix.BuildingBlocks.Data.Uow.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.BuildingBlocks.Data.Uow.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册工作单元服务
        /// </summary>
        /// <typeparam name="TContext">工作单元上下文类型</typeparam>
        /// <param name="builder">服务集合</param>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection builder)
        where TContext : DbContext
        {
            builder.AddScoped<IUnitOfWork>(sp =>
            {
                return new UnitOfWork(sp.GetRequiredService<TContext>());
            });
            return builder;
        }
    }
}