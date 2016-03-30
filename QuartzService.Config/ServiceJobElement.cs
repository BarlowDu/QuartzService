using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace QuartzService.Config
{
    public class ServiceJobElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {

            get
            {
                return base["name"].ToString();
            }
            set
            {
                base["name"] = value;
            }
        }
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return base["type"].ToString();
            }
            set
            {
                base["type"] = value;
            }
        }


        [ConfigurationProperty("method", IsRequired = true)]
        public string Method
        {
            get
            {
                return base["method"].ToString();
            }
            set
            {
                base["method"] = value;
            }
        }


        [ConfigurationProperty("cron")]
        public string Cron
        {
            get
            {
                return base["cron"].ToString();
            }
            set
            {
                base["cron"] = value;
            }
        }


    }
}
