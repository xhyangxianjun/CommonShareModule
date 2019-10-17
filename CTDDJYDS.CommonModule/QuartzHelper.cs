using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTDDJYDS.CommonModule
{
    public class QuartzHelper
    {
        static readonly IScheduler _scheduler;
        static QuartzHelper()
        {
            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型

            //创建一个工厂
            var schedulerFactory = new StdSchedulerFactory(properties);
            //启动
            _scheduler = schedulerFactory.GetScheduler().Result;
            //1、开启调度
            _scheduler.Start();
        }
        /// <summary>
        /// 时间间隔执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="seconds">时间间隔(单位：秒)</param>
        public static async Task<bool> ExecuteInterval<T>(int seconds) where T : IJob
        {
            //2、创建工作任务
            IJobDetail job = JobBuilder.Create<T>().Build();
            // 3、创建触发器
            ITrigger trigger = TriggerBuilder.Create()
           .StartNow()
           .WithSimpleSchedule(
                x => x.WithIntervalInSeconds(seconds)
                //x.WithIntervalInMinutes(1)
                .RepeatForever())
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
        public static async Task<bool> ExecuteByCron<T>(string cronExpression) where T : IJob
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
            IJobDetail job = JobBuilder.Create<T>().Build();
            //3、创建触发器
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
            .StartNow()
            .WithCronSchedule(cronExpression)
            .Build();
            //4、将任务加入到任务池
            await _scheduler.ScheduleJob(job, trigger);
            return true;
        }
    }
}
