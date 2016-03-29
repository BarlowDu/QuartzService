using Quartz;
using QuartzService.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService
{
    [DisallowConcurrentExecution]
    public class DefaultJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            var section = ServiceSection.GetSection("quartzservice");
            var invoker = section.GetJobInvoker(0);
            if (invoker != null)
            {
                invoker.Method.Invoke(invoker.Instance, null);
            }
        }
    }
}
