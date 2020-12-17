using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerManager
{
    public interface IJobBase : IJob
    {
        string JobGroup { get; set; }
        string JobName { get; set; }
        string CronStr { get; set; }
        string JobDescription { get; set; }

    }
}
