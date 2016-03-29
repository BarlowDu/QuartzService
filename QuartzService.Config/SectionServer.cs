using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Config
{
    public class ServiceSection
    {
        public static QuartzServiceSection GetSection(string sectionName)
        {
            return (QuartzServiceSection)ConfigurationManager.GetSection(sectionName);
        }

    }
}
