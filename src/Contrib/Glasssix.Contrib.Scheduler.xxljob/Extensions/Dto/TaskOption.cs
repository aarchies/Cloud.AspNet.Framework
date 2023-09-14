using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2
    }

    public class Result
    {
        public Result(StateCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public StateCode Code { get; set; }
        public string Message { get; set; }
    }

    public class Result<T> : Result
        where T : class
    {
        public Result(StateCode code, string message, T data = null) : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 任务详细参数
    /// </summary>
    public class TaskDetailed
    {
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addTime { get; set; }

        /// <summary>
        /// 报警邮件
        /// </summary>
        public string alarmEmail { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 阻塞处理策略
        /// </summary>
        public string executorBlockStrategy { get; set; }

        /// <summary>
        /// 失败重试次数
        /// </summary>
        public string executorFailRetryCount { get; set; }

        /// <summary>
        /// 执行器Handles
        /// </summary>
        public string executorHandler { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        public string executorParam { get; set; }

        /// <summary>
        /// 路由策略
        /// </summary>
        public string executorRouteStrategy { get; set; }

        /// <summary>
        /// 任务超时时间
        /// </summary>
        public string executorTimeout { get; set; }

        /// <summary>
        /// 运行模式备注
        /// </summary>
        public string glueRemark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string glueSource { get; set; }

        /// <summary>
        /// 运行模式
        /// </summary>
        public string glueType { get; set; }

        /// <summary>
        /// 执行修改时间
        /// </summary>
        public string glueUpdatetime { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string jobDesc { get; set; }

        /// <summary>
        /// 任务分组Id
        /// </summary>
        public int jobGroup { get; set; }

        /// <summary>
        /// 调度过期策略
        /// </summary>
        public string misfireStrategy { get; set; }

        /// <summary>
        /// Cron参数/固定速度值
        /// </summary>
        public string scheduleConf { get; set; }

        /// <summary>
        /// 调度类型
        /// </summary>
        public string scheduleType { get; set; }

        /// <summary>
        /// 最后触发时间
        /// </summary>
        public long triggerLastTime { get; set; }

        /// <summary>
        /// 触发下一次时间
        /// </summary>
        public long triggerNextTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int triggerStatus { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updateTime { get; set; }

        //public string ChildJobId { get; set; }
    }

    public class TaskOption
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public List<TaskDetailed> Data { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordsTotal { get; set; }
    }
}