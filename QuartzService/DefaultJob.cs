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
            var invoker = section.GetJobInvoker(context.JobDetail.Key.Name);
            if (invoker == null)
                throw new NullReferenceException("job服务实例获取失败.");

            invoker.Method.Invoke(invoker.Instance, null);
        }
    }
}
