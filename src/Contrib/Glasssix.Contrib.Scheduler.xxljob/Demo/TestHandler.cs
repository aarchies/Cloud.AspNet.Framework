using DotXxlJob.Core;
using DotXxlJob.Core.Model;
using System;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.xxljob.Demo
{
    [JobHandler("TestHandler")]
    public class TestHandler : AbstractJobHandler
    {
        /// <summary>
        /// 业务Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ReturnT> Execute(JobExecuteContext context)
        {
            Console.WriteLine($"哈哈哈1");
            Console.WriteLine($"哈哈哈2");
            Console.WriteLine($"哈哈哈3");
            return await Task.FromResult(ReturnT.SUCCESS);
        }
    }
}