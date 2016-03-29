using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler1
{
    public class DemoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Process process = Process.GetCurrentProcess();
            process.Exited += process_Exited;
            Console.WriteLine("PID:{0}\tPName:{1}\t{2:yyyy-MM-dd HH:mm:ss}", process.Id, process.ProcessName, DateTime.Now);

        }

        void process_Exited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

    }
}
