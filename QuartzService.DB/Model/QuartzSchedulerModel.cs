using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.DB.Model
{
    public class QuartzSchedulerModel
    {
        /*
         [SchedulerId] [int] NOT NULL,
[SchedulerName] [nvarchar] (150) COLLATE Chinese_PRC_CI_AS NULL,
[Directory] [nvarchar] (150) COLLATE Chinese_PRC_CI_AS NULL,
[FileName] [nvarchar] (150) COLLATE Chinese_PRC_CI_AS NULL,
[Port] [int] NULL,
[IsEnable] [bit]
         */

        public int SchedulerId { get; set; }
        public string SchedulerName { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public int Port { get; set; }
        public bool IsEnable { get; set; }
    }
}
