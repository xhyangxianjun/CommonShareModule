
using CTDDJYDS.CommonModule;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerManager
{
    public class TestJob : IJobBase
    {
        public string JobGroup { get; set; }
        public string JobName { get; set; }
        public string CronStr { get; set; }
        public string JobDescription { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            Thread.Sleep(3000);
            return new Task(() => LogHelper.Log("测试TestJob"));
        }
    }
}
