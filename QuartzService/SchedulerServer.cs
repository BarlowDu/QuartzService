using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzService.Config;
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

            var section = ServiceSection.GetSection("quartzservice");
            var jobConfigs = section.Jobs;
            ISchedulerFactory factory = new StdSchedulerFactory();
            Scheduler = factory.GetScheduler();
            foreach (var j in jobConfigs)
            {
                var jConfig = (j as ServiceJobElement);
                string jobName = jConfig.Name;
                string triggerName = jobName + "_trigger";
                string cron = jConfig.Cron;
                var job = JobBuilder.Create<DefaultJob>()
                    .WithIdentity(jobName, "jobgroup")
                    .RequestRecovery(true).Build();
                ITrigger trigger;

                if (string.IsNullOrEmpty(cron))
                {

                    trigger = TriggerBuilder.Create()
                    .WithIdentity("triggergroup", triggerName)
                    .StartAt(DateTimeOffset.Now)
                    .Build();
                }
                else
                {
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("localTrigger", triggerName)
                    .StartAt(DateTimeOffset.Now)
                    .WithCronSchedule(cron)
                    .Build();

                }

                if (!Scheduler.CheckExists(job.Key))
                {
                    Scheduler.ScheduleJob(job, trigger);
                }
                Scheduler.ListenerManager.AddJobListener(new DefaultJobListener(), KeyMatcher<JobKey>.KeyEquals(job.Key));
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
