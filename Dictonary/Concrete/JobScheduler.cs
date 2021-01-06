using log4net;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NewsPortal.Concrete
{
    public class JobScheduler
    {
        static ILog Log = log4net.LogManager.GetLogger(typeof(JobScheduler));
        public static async Task StartAsync()
        {
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                   { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);

                // get a scheduler
                IScheduler scheduler = await factory.GetScheduler();
                await scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<JobClass>()
                    .WithIdentity("myJob", "group1")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(10)
                .RepeatForever())
                .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}