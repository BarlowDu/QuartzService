using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Config
{
    public class QuartzServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("server", IsRequired = true)]
        public SchedulerServerElement Server
        {
            get { return (SchedulerServerElement)base["server"]; }
        }
        [ConfigurationProperty("jobs", IsDefaultCollection = true)]
        public ServiceJobElementCollection Jobs
        {
            get
            {
                return (ServiceJobElementCollection)base["jobs"];

            }
        }

        public ServerInvoker GetServerInvoker()
        {
            ServerInvoker result = new ServerInvoker();
            Type type = Type.GetType(Server.Type);
            if (type == null)
            {
                return null;
            }
            result.Instance = Activator.CreateInstance(type);
            result.Method = type.GetMethod(Server.Method);
            return result;

        }

        public JobInvoker GetJobInvoker(int index)
        {
            var job = Jobs[index];
            JobInvoker result = new JobInvoker();
            Type type = Type.GetType(job.Type);
            if (type == null)
            {
                return null;
            }
            result.Instance = Activator.CreateInstance(type);
            result.Method = type.GetMethod(job.Method);
            return result;


        }

        public JobInvoker GetJobInvoker(string name)
        {
            var job = Jobs[name];
            JobInvoker result = new JobInvoker();
            Type type = Type.GetType(job.Type);
            if (type == null)
            {
                return null;
            }
            result.Instance = Activator.CreateInstance(type);
            result.Method = type.GetMethod(job.Method);
            return result;


        }

    }
}
