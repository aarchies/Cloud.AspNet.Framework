using Dapper;
using Dapper.Contrib.Extensions;
using Glasssix.Contrib.Data.Orm.Dapper.Abstaractions;
using Glasssix.Contrib.Data.Orm.Dapper.Extensions;
using Glasssix.Contrib.Data.Orm.Dapper.Runtime;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Orm
{
    public class DapperHelper<IT> : IDapper<IT>
    {
        public string _connectionString;
        private readonly ILogger<DapperHelper<IT>> _logger;

        public DapperHelper(string connectionString, IAutoRuntimeServices autoRuntimeServices, ILoggerFactory loggerFactory)
        {
            _connectionString = connectionString;
            _autoRuntimeServices = autoRuntimeServices;
            _logger = loggerFactory.CreateLogger<DapperHelper<IT>>();
        }

        public IAutoRuntimeServices _autoRuntimeServices { get; set; }

        #region Dapper Base

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task DeleteAsync<T>(T entity, string? tableName = null) where T : class
        {
            if (entity is null) return;
            using IDbConnection conn = new MySqlConnection(_connectionString);

            var type = typeof(T);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var keyProperty = properties.Where(x => x.GetCustomAttribute<ExplicitKeyAttribute>() is not null).FirstOrDefault();

            if (keyProperty is null)
            {
                throw new Exception("请指定主键！");
            }

            tableName ??= FindTableName(type);

            var sql = $"delete from `{tableName}` where {keyProperty.Name} = @{keyProperty.Name}";

            await conn.ExecuteAsync(sql);
        }

        public async Task<bool> ExecuteAsync(string cmd, object? param = null, bool beginTransaction = false)
        {
            using IDbConnection conn = new MySqlConnection(_connectionString);

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            var line = await conn.ExecuteAsync(cmd, param, transaction);

            transaction?.Commit();

            return line > 0;
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T>? GetAllInfo<T>() where T : class
        {
            try
            {
                using (IDbConnection conn = new MySqlConnection(_connectionString))
                {
                    var list = conn.GetAll<T>().ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 通过ID获取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T?> GetInfoByIdAsync<T>(string id) where T : class
        {
            try
            {
                using (IDbConnection conn = new MySqlConnection(_connectionString))
                {
                    var list = await conn.GetAsync<T>(id);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="OnSubmeter">是否开启分表</param>
        /// <param name="beginTransaction"></param>
        /// <param name="disposeConn"></param>
        /// <returns></returns>
        public async Task InsertAsync<T>(T? entity = null, bool beginTransaction = false, string? tableName = null) where T : class
        {
            if (entity is null) return;

            using IDbConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            var type = typeof(T);

            tableName ??= FindTableName(type);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var colums = properties.Select(p => $"`{p.Name}`").ToArray();

            var args = properties.Select(x => "@" + x.Name).ToArray();

            var sql = $"INSERT INTO `{tableName}` ( {string.Join(",", colums)}) VALUES ({string.Join(",", args)})";

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            await conn.ExecuteAsync(sql, entity, transaction);

            transaction?.Commit();
        }

        /// <summary>
        /// 异步获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">SQL语句</param>
        /// <param name="param">数据体</param>
        /// <param name="OnSubmeter">是否启用分表</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<T>> QueryAsync<T>(string cmd, object? param = null, bool OnSubmeter = true, CommandType? commandType = null, IDbTransaction? transaction = null) where T : class
        {
            using IDbConnection conn = new MySqlConnection(_connectionString);
            var type = typeof(T);
            var obj = type.GetCustomAttribute(typeof(SubmeterAttribute), false);
            if (obj != null && OnSubmeter == true)
            {
                List<T> values = new List<T>();
                var tablelist = await _autoRuntimeServices.GetRelevantTableNameAsync(type.Name);
                foreach (var item in tablelist)
                {
                    var sqls = OrTable(cmd, item);
                    var data = await conn.QueryAsync<T>(sqls, param, transaction, commandType: commandType ?? CommandType.Text);
                    values.AddRange(data);
                }

                return values;
            }

            var entity = await conn.QueryAsync<T>(cmd, param, transaction, commandType: commandType ?? CommandType.Text);

            return entity;
        }

        public async Task<IEnumerable<TResult>> QueryMultipleAsync<T1, T2, TResult>(string sql, Func<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<TResult>> map, object? param = null)
            where T1 : class
            where T2 : class
            where TResult : class
        {
            using IDbConnection conn = new MySqlConnection(_connectionString);

            using var multi = await conn.QueryMultipleAsync(sql, param);

            var t1 = await multi.ReadAsync<T1>();
            var t2 = await multi.ReadAsync<T2>();

            var result = map.Invoke(t1, t2);

            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">SQL语句</param>
        /// <param name="param">数据体</param>
        /// <param name="OnSubmeter">是否启用分表</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<T>, int)> QueryPageAsync<T>(string cmd, int skip = 0, int limit = 10, object? param = null, bool OnSubmeter = true, CommandType? commandType = null, IDbTransaction? transaction = null) where T : class
        {
            using IDbConnection conn = new MySqlConnection(_connectionString);
            var type = typeof(T);
            var obj = type.GetCustomAttribute(typeof(SubmeterAttribute), false);
            if (obj != null && OnSubmeter == true)
            {
                var sqlAndOrderBy = Regex.Split(cmd, @"[Oo][Rr][Dd][Ee][Rr]\s*[Bb][Yy]");

                var sqlBuilder = new StringBuilder();

                var tablelist = await _autoRuntimeServices.GetRelevantTableNameAsync(type.Name);

                foreach (var item in tablelist)
                {
                    var sqls = OrTable(sqlAndOrderBy[0], item);
                    sqlBuilder.AppendLine(sqls)
                              .AppendLine("union All");
                }

                sqlBuilder.Remove(sqlBuilder.Length - 11, 11);

                var sql = sqlBuilder.ToString();

                var countSql = $"SELECT COUNT(*) as Count FROM({sql}) as a";
                var countModels = await conn.QueryAsync<CountModel>(countSql);

                //排序
                if (sqlAndOrderBy is { Length: > 1 })
                    sqlBuilder.AppendLine($"order by {sqlAndOrderBy[1]}");

                //分页
                sqlBuilder.AppendLine($"limit {skip},{limit}");

                _logger.LogInformation($@"Dapper Logs Cmd{sqlBuilder}");
                var data = await conn.QueryAsync<T>(sqlBuilder.ToString(), param, transaction, commandType: commandType ?? CommandType.Text);

                return (data, countModels.First().Count);
            }

            var entity = await conn.QueryAsync<T>(cmd, param, transaction, commandType: commandType ?? CommandType.Text);

            return (entity, 0);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="OnSubmeter">是否开启分表</param>
        /// <param name="beginTransaction"></param>
        /// <param name="disposeConn"></param>
        /// <returns></returns>
        public async Task UpdateAsync<T>(T entity, bool beginTransaction = false, string? tableName = null) where T : class
        {
            if (entity is null) return;
            var type = typeof(T);

            tableName ??= FindTableName(type);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var keyProperty = properties.Where(x => x.GetCustomAttribute<ExplicitKeyAttribute>() is not null).FirstOrDefault();

            if (keyProperty is null)
                throw new Exception("请指定主键！");

            var colums = properties
                .Where(x => x.Name != keyProperty.Name)
                .Select(p => $"`{p.Name}` = @{p.Name}")
                .ToArray();

            var upsql = $"UPDATE  `{tableName}` SET {string.Join(",", colums)} where `{keyProperty.Name}`=@{keyProperty.Name}";

            using IDbConnection conn = new MySqlConnection(_connectionString);

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            await conn.ExecuteAsync(upsql, entity, transaction);

            transaction?.Commit();
        }

        #endregion Dapper Base

        #region DapperPuls List

        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async ValueTask<bool> DeleteListAsync<T>(IEnumerable<T> entitys, string? tableName = null, bool beginTransaction = false) where T : class
        {
            if (entitys.Count() == 0) return true;
            var type = typeof(T);

            tableName ??= FindTableName(type);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var keyProperty = properties.Where(x => x.GetCustomAttribute<ExplicitKeyAttribute>() is not null).FirstOrDefault();

            if (keyProperty is null)
            {
                throw new Exception("请指定主键！");
            }

            var ids = entitys.Select(x =>
            {
                var v = type.GetProperty(keyProperty.Name)!.GetValue(x);

                if (v is string)
                {
                    return $"'{v}'";
                }
                return v;
            });

            var sql = $"delete from `{tableName}` where {keyProperty.Name} in ({string.Join(",", ids)})";

            using IDbConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            var rows = await conn.ExecuteAsync(sql, null, transaction);

            transaction?.Commit();

            return rows > 0;
        }

        /// <summary>
        /// 批量新增信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async ValueTask<bool> InsertListAsync<T>(IEnumerable<T> entities, string? tableName = null, bool beginTransaction = false) where T : class
        {
            var list = entities.ToList();
            if (list.Count == 0) return true;

            var type = typeof(T);
            tableName ??= FindTableName(type);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var colums = properties.Select(p => $"`{p.Name}`").ToArray();

            var sqlBuilder = new StringBuilder($"INSERT INTO `{tableName}` ( {string.Join(",", colums)}) VALUES ");

            using var conn = new MySqlConnection(_connectionString);

            using var cmd = conn.CreateCommand();

            for (int i = 0; i < list.Count; i++)
            {
                var args = properties.Select(x =>
                {
                    cmd.Parameters.Add(new MySqlParameter($"{x.Name}{i}", x.GetValue(list[i])));
                    return $"@{x.Name}{i}";
                }).ToArray();

                var line = $"({string.Join(",", args)})";

                if (i < list.Count - 1)
                {
                    line += ",";
                }

                sqlBuilder.AppendLine(line);
            }

            conn.Open();

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            var sql = sqlBuilder.ToString();

            cmd.CommandText = sql;

            var rows = await cmd.ExecuteNonQueryAsync();

            transaction?.Commit();

            return rows > 0;
        }

        /// <summary>
        /// 批量修改信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async ValueTask<bool> UpdateListAsync<T>(IEnumerable<T> entities, string? tableName = null, bool beginTransaction = false) where T : class
        {
            if (entities.Count() == 0) return true;
            var type = typeof(T);

            tableName ??= FindTableName(type);

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<WriteAttribute>()?.Write ?? true);

            var keyProperty = properties.Where(x => x.GetCustomAttribute<ExplicitKeyAttribute>() is not null).FirstOrDefault();

            if (keyProperty is null)
                throw new Exception("请指定主键!");

            var colums = properties
                .Where(x => x.Name != keyProperty.Name)
                .Select(p => $"`{p.Name}` = @{p.Name}")
                .ToArray();

            var upsql = $"UPDATE  `{tableName}` SET {string.Join(",", colums)} where {keyProperty.Name}=@{keyProperty.Name}";

            using IDbConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            using IDbTransaction? transaction = beginTransaction ? conn.BeginTransaction() : null;

            var rows = await conn.ExecuteAsync(upsql, entities, transaction);

            transaction?.Commit();

            return rows > 0;
        }

        #endregion DapperPuls List

        #region Dapper Extenions

        /// <summary>
        /// 判读表是否存在
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsTableAsync(string tableName)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IEnumerable<object>? result = null;
            using (IDbConnection conn = new MySqlConnection(_connectionString))
            {
                result = await conn.QueryAsync<object>($@" SELECT *  FROM information_schema.TABLES WHERE table_name ='{tableName}'  and TABLE_SCHEMA = (select database());");
            }
            await Task.Yield();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds + "ms");
            return result.Count() == 0 ? false : true;
        }

        /// <summary>
        /// 分表名称转换
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string OrTable(string cmd, string tableName)
        {
            var cmds = cmd.Split("from".ToArray());
            var prm = cmds[1].Split(" ".ToArray());

            prm[1] = tableName;
            var query = string.Empty;
            for (int i = 0; i < prm.Length; i++)
                query += prm[i].ToString() + " ";

            return cmds[0].ToString() + "from" + query.ToString();
        }

        private string FindTableName(Type type)
        {
            var submeter = type.GetCustomAttribute<SubmeterAttribute>(inherit: false);

            if (submeter is not null)
                return _autoRuntimeServices.GetTableNameAsync(type.Name)!;

            var tab = type.GetCustomAttribute<TableAttribute>(inherit: false);

            if (tab is not null)
                return tab.Name;

            return type.Name;
        }

        #endregion Dapper Extenions
    }
}