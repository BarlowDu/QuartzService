using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class JobModel
    {
        public string GroupName { get; set; }

        public string JobName { get; set; }

        public TriggerState Status { get; set; }
       
    }
}