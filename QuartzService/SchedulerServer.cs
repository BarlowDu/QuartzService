using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzService.Config;
using QuartzService.Container;
using QuartzService.Log;
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
                object jobObj = Activator.CreateInstance(Type.GetType(jConfig.Type));
                JobObjectContainer.Set(jConfig.Name, jobObj);
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
            Scheduler.ListenerManager.AddSchedulerListener(new DefaultSchedulerListener(this));
        }

        public virtual void Start()
        {
            InitScheduler();
            Scheduler.Start();

            LogHandler.Info("Scheduler 开始运行");
        }

        public virtual void Shutdown()
        {
            Action<System.Collections.DictionaryEntry> action = (kv) =>
            {
                if (kv.Value != null)
                {
                    Type type = kv.Value.GetType();
                    if (typeof(IDisposable).IsAssignableFrom(type))
                    {
                        IDisposable obj = (kv.Value as IDisposable);
                        obj.Dispose();
                        Console.WriteLine("Dispose");
                    }
                }

            };

            JobObjectContainer.Clear(action);

            LogHandler.Info("Scheduler 停止运行");
        }


    }
}
