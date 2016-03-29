using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace QuartzService.Web.Models
{

    public class SchedulerModel
    {
        public int SchedulerId { get; set; }

        public string SchedulerName { get; set; }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public int Port { get; set; }

        public SchedulerStatus Status { get; set; }
        public string ProcessName
        {
            get
            {
                if (string.IsNullOrEmpty(FileName) || FileName.Contains('.') == false)
                {
                    return null;
                }
                else
                {
                    return FileName.Substring(0, FileName.IndexOf('.'));
                }
            }
        }

        [ScriptIgnore]
        public Process Process { get; set; }

        [ScriptIgnore]
        public IScheduler Scheduler { get; set; }

        public List<JobModel> Jobs { get; set; }
    }
}