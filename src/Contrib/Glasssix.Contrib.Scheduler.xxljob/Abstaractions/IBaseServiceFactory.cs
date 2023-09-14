using Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.xxljob.Abstaractions
{
    public interface IBaseServiceFactory
    {
        /// <summary>
        /// 获取当前任务组数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<TaskOption>> GetTaskPageList(TaskQueyInput input);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="type">执行类型</param>
        void Invoke(string taskId, TaskStateType type);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="pairs">详细参数</param>
        /// <returns></returns>
        ValueTask<string> TryAdd(VisitorXxlJobOption option);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        ValueTask<xxlJobResult> TryUpdate(VisitorXxlJobOption option);
    }
}