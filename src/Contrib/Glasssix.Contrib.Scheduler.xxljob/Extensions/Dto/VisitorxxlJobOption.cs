using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto
{
    public enum ScheduleType
    {
        None = 0,
        Cron = 1,
        FIX_RATE = 2,
    }

    public class VisitorXxlJobOption
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="type">调度类型</param>
        /// <param name="value">cron：表达式；FIX_RATE：次/秒</param>
        public VisitorXxlJobOption(ScheduleType type, string value)
        {
            switch (type)
            {
                case ScheduleType.None:
                    break;

                case ScheduleType.Cron:
                    scheduleType = "CRON";
                    scheduleConf = value;
                    cronGen_display = value;
                    schedule_conf_CRON = value;
                    break;

                case ScheduleType.FIX_RATE:
                    scheduleType = "FIX_RATE";
                    scheduleConf = value;
                    break;
            }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// Cron
        /// </summary>
        public string cronGen_display { get; set; }

        /// <summary>
        /// 阻塞处理策略
        /// </summary>
        public string executorBlockStrategy { get; set; } = "SERIAL_EXECUTION";

        /// <summary>
        /// 失败重试次数
        /// </summary>
        public string executorFailRetryCount { get; set; } = "0";

        /// <summary>
        /// JobHandler
        /// </summary>
        public string executorHandler { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        public string executorParam { get; set; }

        /// <summary>
        /// 路由策略
        /// </summary>
        public string executorRouteStrategy { get; set; } = "FIRST";

        /// <summary>
        /// 任务超时时间
        /// </summary>
        public string executorTimeout { get; set; } = "0";

        public string glueRemark { get; set; } = "GLUE代码初始化";

        /// <summary>
        /// 运行模式
        /// </summary>
        public string glueType { get; set; } = "BEAN";

        public int id { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string jobDesc { get; set; }

        /// <summary>
        /// 调度过期策略
        /// </summary>
        public string misfireStrategy { get; set; } = "DO_NOTHING";

        /// <summary>
        /// Cron
        /// </summary>
        public string schedule_conf_CRON { get; set; }

        /// <summary>
        /// 固定速度值
        /// </summary>
        public string scheduleConf { get; set; }

        /// <summary>
        /// 调度类型
        /// </summary>
        public string scheduleType { get; set; }

        public Dictionary<string, string> ConvertToDeviceParameterJson()
        {
            var pairs = new Dictionary<string, string>();
            var properties = GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string value;
                if (item.PropertyType.BaseType == typeof(Array))
                    value = Newtonsoft.Json.JsonConvert.SerializeObject(item.GetValue(this, null));
                else
                    value = item.GetValue(this, null)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    pairs.Add(item.Name, value);
            }
            return pairs;
        }
    }
}