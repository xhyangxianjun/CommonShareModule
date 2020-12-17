
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerManager
{
    public class QuartzHelper
    {
        static IScheduler _scheduler;
        public static void  CreateQuartzScheduler(string connStr,string datasource)
        {
            //创建一个作业调度池
            var properties = new NameValueCollection();
            //存储类型：第一种类型叫做RAMJobStore（默认，这种方法提供了最佳的性能，因为内存中数据访问最快，足之处是缺乏数据的持久性），第二种类型叫做JDBC作业存储
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            //表明数据库表的前缀
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            //数据库驱动类型
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";                
            //数据源名称
            properties["quartz.jobStore.dataSource"] = datasource;
            //连接字符串--"server=192.168.2.88;database=QuartzDB;uid=sa;pwd=winner@001"
            properties["quartz.dataSource.myDS.connectionString"] = connStr;
            //sqlserver版本
            // * SqlServer-20         2.0.0.0
            // * SqlServerCe-351      3.5.1.0
            // * SqlServerCe-352      3.5.1.50
            // * SqlServerCe-400      4.0.0.0
            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "10";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            //properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            //properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            //properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";//名称
            //properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            //properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            //properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";

            //是否集群
            //properties["quartz.jobStore.clustered"] = "false";
            // Quartz Scheduler唯一实例ID，auto：自动生成
            //properties["quartz.scheduler.instanceId"] = "AUTO";

            //创建一个工厂
            var schedulerFactory = new StdSchedulerFactory(properties);
            //启动
            _scheduler = schedulerFactory.GetScheduler().Result;
            //1、开启调度
            //_scheduler.Start();
        }

        /// <summary>
        /// 暂停指定任务计划
        /// </summary>
        /// <returns></returns>
        public bool PauseScheduleJob(string jobGroup, string jobName,bool isAll=false)
        {
            try
            {
                if (isAll)
                    _scheduler.PauseAll();
                else
                    _scheduler.PauseJob(new JobKey(jobName, jobGroup));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 开启指定的任务计划
        /// </summary>
        /// <returns></returns>
        public bool RunScheduleJob(string jobGroup, string jobName,bool isAll=false)
        {
            try
            {
                if (!_scheduler.IsStarted)
                    _scheduler.Start();
                if (isAll)
                    _scheduler.ResumeAll();
                else
                    _scheduler.ResumeJob(new JobKey(jobName, jobGroup));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ShopSheduleJob()
        {
            //true:等待任务完成后再结束
            _scheduler.Shutdown(true);
        }

        public void RemoveSheduleJob(string jobGroup, string jobName)
        {
            var trigger = new TriggerKey(jobGroup, jobName);
            _scheduler.PauseTrigger(trigger);//停止触发器
            _scheduler.UnscheduleJob(trigger); //移除触发器
            _scheduler.DeleteJob(JobKey.Create(jobName, jobGroup));
        }

        /// <summary>
        /// 清除job
        /// </summary>
        public static void ClearSchedule()
        {

            try
            {
                //判断调度是否已经关闭
                if (!_scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    _scheduler.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 时间间隔执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="seconds">时间间隔(单位：秒)</param>
        public static async Task<bool> ExecuteInterval<T>(T t, int seconds) where T : IJobBase
        {
            //2、创建工作任务
            IJobDetail job = JobBuilder.Create<T>()
                .WithIdentity(t.JobName, t.JobGroup)
                //.WithDescription(m.JobDescription)
                .Build();
            // 3、创建触发器
            ITrigger trigger = TriggerBuilder.Create()
           .StartNow()
           //.WithCronSchedule("0/5 * * * * ?")     //5秒执行一次
           .WithSimpleSchedule(
                x => x.WithIntervalInSeconds(seconds)
                //x.WithIntervalInMinutes(1)
                .RepeatForever())
           .WithIdentity(t.JobName, t.JobGroup)
           //.WithDescription(m.JobDescription)
           .Build();
            //4、将任务加入到任务池
            await _scheduler.ScheduleJob(job, trigger);
            return true;
        }

        /// <summary>
        /// 指定时间执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="cronExpression">cron表达式，即指定时间点的表达式</param>
        public static async Task<bool> ExecuteByCron<T>(T t, string cronExpression) where T : IJobBase
        {
            /*
             简单说一下Cron表达式:
             由7段构成：秒 分 时 日 月 星期 年（可选）
  
             "-" ：表示范围  MON-WED表示星期一到星期三
             "," ：表示列举 MON,WEB表示星期一和星期三
             "*" ：表是“每”，每月，每天，每周，每年等
             "/" :表示增量：0/15（处于分钟段里面） 每15分钟，在0分以后开始，3/20 每20分钟，从3分钟以后开始
             "?" :只能出现在日，星期段里面，表示不指定具体的值
             "L" :只能出现在日，星期段里面，是Last的缩写，一个月的最后一天，一个星期的最后一天（星期六）
             "W" :表示工作日，距离给定值最近的工作日
             "#" :表示一个月的第几个星期几，例如："6#3"表示每个月的第三个星期五（1=SUN...6=FRI,7=SAT）  
             如果Minutes的数值是 '0/15' ，表示从0开始每15分钟执行  
             如果Minutes的数值是 '3/20' ，表示从3开始每20分钟执行，也就是‘3/23/43’
             */
            //2、创建工作任务
            IJobDetail job = JobBuilder.Create<T>()
                .WithIdentity(t.JobName, t.JobGroup)
                //.WithDescription(m.JobDescription)
                .Build();
            //3、创建触发器
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
            .StartNow()
            .WithCronSchedule(cronExpression)
            .WithIdentity(t.JobName, t.JobGroup)
            //.WithDescription(m.JobDescription)
            .Build();
            //4、将任务加入到任务池
            await _scheduler.ScheduleJob(job, trigger);
            return true;
        }
    }
}
