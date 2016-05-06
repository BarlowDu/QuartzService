using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace QuartzService.Config
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name {

            get
            {
                return base["name"].ToString();
            }
            set
            {
                base["name"] = value;
            }
        }
        [ConfigurationProperty("schedulerType", IsRequired = true)]
        public Type SchedulerType
        {
            get
            {
                return Type.GetType(base["schedulerType"].ToString());
            }
            set
            {
                base["schedulerType"] = value;
            }
        }


        [ConfigurationProperty("schedulerMethod", IsRequired = true)]
        public MethodInfo SchedulerMethod
        {
            get
            {
                return SchedulerType.GetMethod(base["schedulerMethod"].ToString()) ;
            }
            set
            {
                base["schedulerMethod"] = value.Name;
            }
        }


        [ConfigurationProperty("executeType", IsRequired = true)]
        public Type ExecuteType
        {
            get
            {
                return Type.GetType(base["executeType"].ToString());
            }
            set
            {
                base["executeType"] = value;
            }
        }


        [ConfigurationProperty("executeMethod", IsRequired = true)]
        public MethodInfo ExecuteMethod
        {
            get
            {
                return SchedulerType.GetMethod(base["executeMethod"].ToString());
            }
            set
            {
                base["executeMethod"] = value.Name;
            }
        }
    }
}
