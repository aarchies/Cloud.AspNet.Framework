using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace Glasssix.BuildingBlocks.Data.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction?> BeginTransactionAsync();

        /// <summary>
        /// 创建Scope类型事务
        /// </summary>
        /// <returns></returns>
        TransactionScope BeginTransactionScope();

        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// 提交Scope类型事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task CommitTransactionAsync(TransactionScope transaction);

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task CommitTransactionAsync(IDbContextTransaction transaction);

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();

        /// <summary>
        /// 事务回滚 （已托管 用户勿强制操作）
        /// </summary>
        void RollbackTransaction();
    }
}