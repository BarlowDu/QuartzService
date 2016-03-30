using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Config
{
    public class JobInvoker:Invoker
    {
        public string Cron { get; set; }

    }
}
