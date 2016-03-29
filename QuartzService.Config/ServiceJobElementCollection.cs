using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Config
{
    public class ServiceJobElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceJobElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceJobElement)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "job";
            }
        }

        public ServiceJobElement this[int index]
        {
            get
            {

                return (ServiceJobElement)BaseGet(index);
            }
        }

        public ServiceJobElement this[string name]
        {
            get
            {

                return (ServiceJobElement)BaseGet(name);
            }
        }

    }
}
