using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto
{
    /// <summary>
    /// 任务状态类型
    /// </summary>
    public enum TaskStateType
    {
        Remove = -1,//删除
        Stop = 0,//停止
        Start = 1,//启动
        trigger = 2//执行一次
    }

    public class TaskQueyInput
    {
        /// <summary>
        /// 负责人
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 执行器Handles
        /// </summary>
        public string executorHandler { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string jobDesc { get; set; }

        /// <summary>
        /// 任务组
        /// </summary>
        public int jobGroup { get; set; }

        /// <summary>
        /// 单页大小
        /// </summary>
        public int length { get; set; } = 1000;

        /// <summary>
        /// 起始页
        /// </summary>
        public int start { get; set; } = 0;

        /// <summary>
        /// 状态类型 全部 -1，启动 1，停止 0
        /// </summary>
        public int triggerStatus { get; set; } = -1;

        public Dictionary<string, string> ConvertToDeviceParameterJson()
        {
            var pairs = new Dictionary<string, string>();
            var properties = GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string value;
                if (item.PropertyType.BaseType == typeof(Array))
                    value = JsonConvert.SerializeObject(item.GetValue(this, null));
                else
                    value = item.GetValue(this, null)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    pairs.Add(item.Name, value);
            }
            return pairs;
        }
    }
}