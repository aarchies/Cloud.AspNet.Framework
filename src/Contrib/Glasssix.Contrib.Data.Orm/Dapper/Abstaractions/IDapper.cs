using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Orm.Dapper.Abstaractions
{
    public interface IDapper<IT>
    {
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task DeleteAsync<T>(T entity, string? tableName = null) where T : class;

        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<bool> DeleteListAsync<T>(IEnumerable<T> entitys, string? tableName = null, bool beginTransaction = false) where T : class;

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="beginTransaction"></param>
        /// <param name="disposeConn"></param>
        /// <returns></returns>
        Task<bool> ExecuteAsync(string cmd, object? param = null, bool beginTransaction = false);

        Task<bool> ExistsTableAsync(string tableName);

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T>? GetAllInfo<T>() where T : class;

        /// <summary>
        /// 通过ID获取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetInfoByIdAsync<T>(string id) where T : class;

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="OnSubmeter"></param>
        /// <param name="beginTransaction"></param>
        /// <param name="disposeConn"></param>
        /// <returns></returns>
        Task InsertAsync<T>(T? entity = null, bool beginTransaction = false, string? tableName = null) where T : class;

        /// <summary>
        /// 批量新增信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<bool> InsertListAsync<T>(IEnumerable<T> entities, string? tableName = null, bool beginTransaction = false) where T : class;

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
        Task<IEnumerable<T>> QueryAsync<T>(string cmd, object? param = null, bool OnSubmeter = true, CommandType? commandType = null, IDbTransaction? transaction = null) where T : class;

        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> QueryMultipleAsync<T1, T2, TResult>(string sql, Func<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<TResult>> map, object? param = null)
            where T1 : class
            where T2 : class
            where TResult : class;

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">SQL语句</param>
        /// <param name="skip">当前页</param>
        /// <param name="limit">显示条目</param>
        /// <param name="param">数据体</param>
        /// <param name="OnSubmeter">是否启用分表</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<(IEnumerable<T>, int)> QueryPageAsync<T>(string cmd, int skip = 1, int limit = 10, object? param = null, bool OnSubmeter = true, CommandType? commandType = null, IDbTransaction? transaction = null) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="OnSubmeter"></param>
        /// <param name="beginTransaction"></param>
        /// <param name="disposeConn"></param>
        /// <returns></returns>
        Task UpdateAsync<T>(T entity, bool beginTransaction = false, string? tableName = null) where T : class;

        /// <summary>
        /// 批量修改信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<bool> UpdateListAsync<T>(IEnumerable<T> entities, string? tableName = null, bool beginTransaction = false) where T : class;
    }

    public class CountModel
    {
        public int Count { get; set; }
    }
}