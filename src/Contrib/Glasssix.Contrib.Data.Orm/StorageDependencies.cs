using Abstaractions;
using Glasssix.Contrib.Data.Orm.Dapper.Abstaractions;
using Glasssix.Contrib.Data.Orm.Dapper.Extensions;
using Glasssix.Contrib.Data.Orm.Dapper.Runtime;
using Glasssix.Utils.ReflectionConductor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Orm
{
    /// <summary>
    /// 数据库持久化拓展
    /// </summary>
    public static class StorageDependencies
    {
        /// <summary>
        /// 反射引导器
        /// </summary>
        public static ITypeFinder TypeFinder = new TypeFinder();

        #region Dapper


        /// <summary>
        /// Dapper
        /// </summary>
        /// <typeparam name="T">自定义实例类型</typeparam>
        /// <param name="services"></param>
        /// <param name="connection">连接字符串</param>
        /// <param name="IsAutoTableCreate">是否启用分表</param>
        /// <param name="OverdueDelete">是否自动清除过期表</param>
        /// <returns></returns>
        public static IServiceCollection AddDapper<T>(this IServiceCollection services, string connection, bool IsAutoTableCreate, bool OverdueDelete = false)
        {
            services.AddSingleton<IAutoRuntimeServices, AutoRuntimeServices>();
            services.AddSingleton<IDapper<T>, DapperHelper<T>>(sp =>
            {
                var runtime = sp.GetRequiredService<IAutoRuntimeServices>();

                if (IsAutoTableCreate)
                {
                    OnsubmterTable(runtime);
                    runtime.AutoCreate(OverdueDelete);
                }
                return new DapperHelper<T>(connection, runtime, sp.GetRequiredService<ILoggerFactory>());
            });

            return services;
        }


        /// <summary>
        /// 分表事件
        /// </summary>
        public static void OnsubmterTable(IAutoRuntimeServices? _autoRuntimeServices = null)
        {
            if (_autoRuntimeServices != null)
            {
                var typesList = new List<string>();
                var ok = TypeFinder.GetAssemblies();
                foreach (var item in ok)
                {
                    if (item.FullName!.Contains("MySql.EntityFrameworkCore") || item.FullName.Contains("System.DirectoryServices.Protocols"))
                        break;
                    var types = item.GetTypes();

                    foreach (var type in types)
                    {
                        var atts = type.GetCustomAttributes(typeof(SubmeterAttribute), false);
                        if (atts != null && atts.Length > 0)
                        {
                            var att = ((SubmeterAttribute[])atts)[0].Time;
                            var properties = type.GetProperties();
                            var dis = new Dictionary<string, string>();
                            foreach (var items in properties)
                            {
                                var clounmName = items.Name;
                                var clounmType = items.PropertyType.Name;
                                dis.Add(clounmName, clounmType);
                            }
                            _autoRuntimeServices.AddDataClounmConcurrent(type.Name, dis);
                            _autoRuntimeServices.AddDataSourcesConcurrent(type.Name, att);
                            typesList.Add(type.Name);
                        }
                    }
                }
            }
        }

        #endregion Dapper

    }
}