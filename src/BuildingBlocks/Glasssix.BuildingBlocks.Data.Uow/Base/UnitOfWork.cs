using Microsoft.EntityFrameworkCore;

namespace Glasssix.BuildingBlocks.Data.Uow.Base
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        public UnitOfWork(DbContext options) : base(options)
        {
        }
    }
}