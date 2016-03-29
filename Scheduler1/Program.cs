using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.Runtime.InteropServices;

namespace Scheduler1
{
    class Program
    {

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]

        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]

        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static void HideWindow()
        {
            IntPtr a = FindWindow("ConsoleWindowClass", Console.Title);

            if (a != IntPtr.Zero)
            {
                ShowWindow(a, 0); //隐藏这个窗口
            }
        }

        static void Main(string[] args)
        {
            //HideWindow();
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler();
            var job = JobBuilder.Create<DemoJob>()
                .WithIdentity("localJob", "default")
                .RequestRecovery(true).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("localTrigger", "default")
                .StartAt(DateTimeOffset.Now)
                .WithCronSchedule("0/5 * * * * ?")
                .Build();
            
            if (!scheduler.CheckExists(job.Key))
            {
                scheduler.ScheduleJob(job, trigger);
            }
            scheduler.Start();
            scheduler.ResumeAll();
            Console.ReadKey();
            scheduler.Shutdown();
            Console.WriteLine("scheduler stopped");
            GC.Collect();
            Console.ReadKey();
            
        }
    }
}
