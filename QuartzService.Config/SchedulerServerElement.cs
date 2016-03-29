using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Config
{
    public class SchedulerServerElement:ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
            public string Type {
                get { return base["type"].ToString(); }
                set { base["type"] = value; }
        
        }

        [ConfigurationProperty("method", IsRequired = true)]
        public string Method
        {
            get { return base["method"].ToString(); }
            set { base["method"] = value; }

        }
    }
}
