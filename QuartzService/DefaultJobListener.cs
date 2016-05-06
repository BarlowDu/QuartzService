using Quartz;
using QuartzService.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService
{
    public class DefaultJobListener : IJobListener
    {
        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (jobException == null)
            {
                //Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss}\t{1}:{2}\tRunned", DateTime.Now, context.JobDetail.Key.Group, context.JobDetail.Key.Name);
            }
            else
            {

                LogHandler.Info("Job运行异常", jobException);
                //Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss}\t{1}:{2}\tthrow Exception:{3}", DateTime.Now, context.JobDetail.Key.Group, context.JobDetail.Key.Name,jobException);
            }
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }
    }
}
