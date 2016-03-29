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

        public Invoker GetServerInvoker()
        {
            Invoker result = new Invoker();
            Type type = Type.GetType(Server.Type);
            if (type == null)
            {
                return null;
            }
            result.Instance = Activator.CreateInstance(type);
            result.Method = type.GetMethod(Server.Method);
            return result;

        }

        public Invoker GetJobInvoker(int index)
        {
            var job = Jobs[index];
            Invoker result = new Invoker();
            Type type = Type.GetType(job.Type);
            if (type == null)
            {
                return null;
            }
            result.Instance = Activator.CreateInstance(type);
            result.Method = type.GetMethod(job.Method);
            return result;


        }

        public Invoker GetJobInvoker(string name)
        {
            var job = Jobs[name];
            Invoker result = new Invoker();
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
