using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace QuickBootstrap.App_Start
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            Debug.WriteLine("default Scheduler is:" + scheduler.SchedulerName);
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<PerformanceExportJob>().Build();

            // 间隔24小时，每天1点执行
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(24)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(1, 0))
            //      )
            //    .Build();

            //  每隔1h 重复执行
            ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    //.WithSimpleSchedule(x => x
                    //    .WithIntervalInSeconds(10)
                    //   .RepeatForever())
                    .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void Stop()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            Debug.WriteLine("default Scheduler is:" + scheduler.SchedulerName);
            scheduler.Shutdown(true);
        }

    }
}