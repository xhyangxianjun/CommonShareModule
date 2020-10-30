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
    public class JobTest : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Thread.Sleep(3000);
                return new Task(() => LogHelper.Log("测试TestJob"));

            }
            catch (Exception ex)
            {

                JobExecutionException e2 = new JobExecutionException(ex);
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
                return null;
            }
        }
    }
}
