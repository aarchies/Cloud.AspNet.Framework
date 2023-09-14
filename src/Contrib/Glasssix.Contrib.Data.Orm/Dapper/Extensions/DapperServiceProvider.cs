using Abstaractions;
using Glasssix.Contrib.Data.Orm.Dapper.Abstaractions;
using Glasssix.Contrib.Data.Orm.Dapper.Runtime;
using Glasssix.Utils.ReflectionConductor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Orm.Dapper.Extensions
{
    public static class DapperServiceProvider
    {
        public static ITypeFinder TypeFinder = new TypeFinder();

        /// <summary>
        /// 启用Dapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapper(this IServiceCollection services, string connection)
        {
            var type = typeof(object);
            services.AddSingleton<IAutoRuntimeServices, AutoRuntimeServices>();
            services.AddSingleton<IDapper<object>, DapperHelper<object>>(sp =>
            {
                return new DapperHelper<object>(connection, sp.GetRequiredService<IAutoRuntimeServices>(), sp.GetRequiredService<ILoggerFactory>());
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
                    if (item.FullName.Contains("MySql.EntityFrameworkCore") || item.FullName.Contains("System.DirectoryServices.Protocols"))
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
                            _autoRuntimeServices.AddDataSourcesConcurrent(type.Name, att!);
                            typesList.Add(type.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 启用分表 并定时执行创建表操作
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void UseAutoDapperTableCreate(this IServiceProvider serviceProvider, bool OverdueDelete = false)
        {
            var runtime = serviceProvider.GetRequiredService<IAutoRuntimeServices>();
            OnsubmterTable(runtime);
            runtime.AutoCreate(OverdueDelete);
        }
    }
}