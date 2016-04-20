using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService
{
    public class DefaultSchedulerListener : SchedulerListenerSupport
    {
        SchedulerServer server;
        public DefaultSchedulerListener(SchedulerServer _server)
        {
            server = _server;
        }
        public override void SchedulerShutdown()
        {
            server.Shutdown();
        }
    }
}
