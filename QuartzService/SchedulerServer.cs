using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService
{
    public class SchedulerServer
    {
        protected IScheduler Scheduler { get; set; }

        public virtual void InitScheduler()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            Scheduler = factory.GetScheduler();
            var job = JobBuilder.Create<DefaultJob>()
                .WithIdentity("localJob", "default")
                .RequestRecovery(true).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("localTrigger", "default")
                .StartAt(DateTimeOffset.Now)
                .WithCronSchedule("*/5 * * * * ?")
                .Build();

            if (!Scheduler.CheckExists(job.Key))
            {
                Scheduler.ScheduleJob(job, trigger);
            }
        }

        public virtual void Start()
        {
            InitScheduler();
            Scheduler.Start();
        }

        public virtual void Shutdown()
        { }


    }
}
