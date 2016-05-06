using Quartz;
using QuartzService.Config;
using QuartzService.Container;
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
            var name = context.JobDetail.Key.Name;
            var invoker = section.GetJobInvoker(name, JobObjectContainer.Get(name));
            if (invoker == null)
                throw new NullReferenceException("job服务实例获取失败.");

            invoker.Method.Invoke(invoker.Instance, null);
        }
    }
}
