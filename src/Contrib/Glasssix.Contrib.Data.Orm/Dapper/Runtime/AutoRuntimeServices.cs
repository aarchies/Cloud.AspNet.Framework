using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Orm.Dapper.Runtime
{
    /// <summary>
    /// 分表核心实现
    /// </summary>
    public class AutoRuntimeServices : IAutoRuntimeServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutoRuntimeServices> _logger;
        private ConcurrentDictionary<string, Dictionary<string, string>> _DataClounm;
        private ConcurrentDictionary<string, string> _DataSources;

        public AutoRuntimeServices(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _DataSources = new ConcurrentDictionary<string, string>();
            _DataClounm = new ConcurrentDictionary<string, Dictionary<string, string>>();
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<AutoRuntimeServices>();
        }

        public void AddDataClounmConcurrent(string name, Dictionary<string, string> dis)
        {
            _DataClounm.TryAdd(name, dis);
        }

        public void AddDataSourcesConcurrent(string name, string tableIndex)
        {
            _DataSources.TryAdd(name, tableIndex);
        }

        public Task AutoCreate(bool OverdueDelete)
        {
            _logger.LogInformation("Monitoring Table AutoCreate....");
            Task.Factory.StartNew(async () =>
           {
               if (_DataSources.Count > 0)
               {
                   while (true)
                   {
                       foreach (var item in _DataSources)
                       {
                           var timenum = DateTime.Now.ToString("yyyyMMdd");
                           if (Convert.ToInt32(item.Value) < Convert.ToInt32(timenum))
                           {
                               await CreateTableJob(item.Key, timenum);
                               if (OverdueDelete)
                                   await DeleteTableJob(item.Key);
                               _DataSources.TryUpdate(item.Key, timenum, item.Value);
                           }
                       }
                       Thread.Sleep(TimeSpan.FromDays(20));
                   }
               }
           });

            return Task.CompletedTask;
        }

        public async Task<List<string>> GetRelevantTableNameAsync(string name)
        {
            var connectionString = _configuration["ConnectionString"];

            using (IDbConnection conn = new MySqlConnection(connectionString))
            {
                var result = await conn.QueryAsync<string>($@"select TABLE_NAME from information_schema.TABLES where TABLE_NAME like '{name}%' and TABLE_SCHEMA = (select database());");
                return result.ToList();
            }
        }

        public string? GetTableNameAsync(string name)
        {
            var connectionString = _configuration["ConnectionString"];
            var num = string.Empty;
            _DataSources.TryGetValue(name, out num);
            IEnumerable<string>? result = null;
            using (IDbConnection conn = new MySqlConnection(connectionString))
            {
                var monTable = name + "_" + num.Remove(num.Length - 2, 2);
                result = conn.Query<string>($@" SELECT TABLE_NAME  FROM information_schema.TABLES WHERE table_name LIKE '{monTable}%'  and TABLE_SCHEMA = (select database());");
            }
            if (result.Count() >= 1)
                return result.FirstOrDefault();

            return default;
        }

        /// <summary>
        /// 转换@sql
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ToSqlClounm(string name)
        {
            var dis = new Dictionary<string, string>();
            _DataClounm.TryGetValue(name, out dis);
            StringBuilder builder = new StringBuilder();
            if (dis.Count > 0)
                foreach (var item in dis)
                {
                    builder.Append("@" + item.Key + ",");
                }

            var result = builder.ToString();
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        public string ToUpdateModelSql(string name)
        {
            var dis = new Dictionary<string, string>();
            _DataClounm.TryGetValue(name, out dis);
            StringBuilder builder = new StringBuilder();
            if (dis.Count > 0)
                foreach (var item in dis)
                {
                    if (item.Key != "Id")
                        builder.Append(item.Key + "=@" + item.Key + ",");
                }

            var result = builder.ToString();
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        /// <summary>
        /// 表创建
        /// </summary>
        /// <param name="tableName"></param>
        private async Task CreateTableJob(string tableName, string timeUnm)
        {
            var connectionString = _configuration["ConnectionString"];
            StringBuilder builder = new StringBuilder();
            var dis = new Dictionary<string, string>();
            _DataClounm.TryGetValue(tableName, out dis);
            if (dis != null)
            {
                var tableNameNow = tableName + "_" + timeUnm;
                _logger.LogInformation($"Event Table {tableNameNow} Createing");
                builder.Append($@"CREATE TABLE `{tableNameNow}`  (");

                foreach (var item in dis)
                {
                    if (item.Key == "Id")
                    {
                        builder.Append($"`{item.Key}`  bigint(20) NOT NULL AUTO_INCREMENT,");
                        break;
                    }
                    var lin = string.Empty;
                    switch (item.Value)
                    {
                        case "String":
                            lin = " longtext,";
                            break;

                        case "Int32":
                            lin = " int,";
                            break;

                        case "Int64":
                            lin = "  bigint(20) NOT NULL,";
                            break;

                        case "DateTime":
                            lin = " datetime(6) NOT NULL,";
                            break;

                        case "Single":
                            lin = " float NOT NULL,";
                            break;

                        default:
                            break;
                    }
                    builder.Append($"`{item.Key}` {lin}");
                }
                builder.Append(" PRIMARY KEY (`Id`) USING BTREE,INDEX `IX_Record_Id`(`Id`) USING BTREE) ENGINE = InnoDB AUTO_INCREMENT = 15889288857958705 CHARACTER SET = utf8mb4; SET FOREIGN_KEY_CHECKS = 1; ");

                //当月是否存在该表 不存在则执行
                IEnumerable<object>? result = null;
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    var monTable = tableNameNow.Remove(tableNameNow.Length - 2, 2);
                    result = await conn.QueryAsync<object>($@" SELECT *  FROM information_schema.TABLES WHERE table_name LIKE '{monTable}%'  and TABLE_SCHEMA = (select database());");
                }
                await Task.Yield();
                if (result.Count() == 0)
                {
                    using (IDbConnection conn = new MySqlConnection(connectionString))
                    {
                        await conn.ExecuteAsync(builder.ToString());
                        _logger.LogInformation($"Event Table {tableNameNow} Createed");
                    }
                }
                else
                    _logger.LogInformation($"Table {tableNameNow} Already Exists!");
            }
        }

        /// <summary>
        /// 删除历史表(保留2个月)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private async Task DeleteTableJob(string tableName)
        {
            var connectionString = _configuration["ConnectionString"];
            var timenum = tableName + "_" + DateTime.Now.AddMonths(-2).ToString("yyyyMMdd");

            IEnumerable<object>? result = null;
            using (IDbConnection conn = new MySqlConnection(connectionString))
            {
                var monTable = timenum.Remove(timenum.Length - 2, 2);
                result = await conn.QueryAsync<string>($@" SELECT TABLE_NAME  FROM information_schema.TABLES WHERE table_name LIKE '{monTable}%'  and TABLE_SCHEMA = (select database());");
            }
            await Task.Yield();
            if (result.Count() != 0)
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.ExecuteAsync($@"drop table {string.Join(",", result)}");
                    _logger.LogInformation($"Event Table {timenum} Deleted");
                }
            }
        }
    }
}