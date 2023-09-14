using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace Glasssix.BuildingBlocks.Data.Uow.Base
{
    /// <summary>
    /// 工作单元基类
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region 字段

        private readonly DbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId { get; set; }

        #endregion 属性

        #region 静态构造方法

        /// <summary>
        /// 初始化Entity Framework工作单元
        /// </summary>
        static UnitOfWorkBase()
        {
        }

        /// <summary>
        /// 初始化Entity Framework工作单元
        /// </summary>
        /// <param name="options">配置</param>
        protected UnitOfWorkBase(DbContext options)
        {
            _dbContext = options;
            TraceId = Guid.NewGuid().ToString();
        }

        #endregion 静态构造方法

        #region UNIT构造方法

        #region BeginTransaction

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            await strategy.ExecuteAsync(async () =>
                {
                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch
                    {
                        RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        if (_currentTransaction != null)
                        {
                            _currentTransaction.Dispose();
                            _currentTransaction = null;
                        }
                    }
                });
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion BeginTransaction

        #region TransactionScope

        public TransactionScope BeginTransactionScope()
        {
            return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public async Task CommitTransactionAsync(TransactionScope transaction)
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                transaction.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion TransactionScope

        #endregion UNIT构造方法

        #region 获取元数据

        public virtual async Task CommitAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("保存实体时出错：" + e.Message);
            }
        }

        public void Dispose()
        {
            _dbContext.DisposeAsync();
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="property">属性名</param>
        public string? GetColumn(Type type, string property)
        {
            if (type == null || string.IsNullOrWhiteSpace(property))
                return null;
            try
            {
                var entityType = _dbContext.Model.FindEntityType(type);
                var result = entityType?.GetProperty(property)?.FindAnnotation("Relational:ColumnName")?.Value!
                    .ToString();
                return string.IsNullOrWhiteSpace(result) ? property : result;
            }
            catch
            {
                return property;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public IDbConnection GetConnection()
        {
            return _dbContext.Database.GetDbConnection();
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <param name="type">实体类型</param>
        public string? GetSchema(Type type)
        {
            if (type == null)
                return null;
            try
            {
                var entityType = _dbContext.Model.FindEntityType(type);
                return entityType?.FindAnnotation("Relational:Schema")?.Value!.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="type">实体类型</param>
        public string? GetTable(Type type)
        {
            if (type == null)
                return null;
            try
            {
                var entityType = _dbContext.Model.FindEntityType(type);
                return entityType?.FindAnnotation("Relational:TableName")?.Value!.ToString();
            }
            catch
            {
                return type.Name;
            }
        }

        #endregion 获取元数据
    }
}