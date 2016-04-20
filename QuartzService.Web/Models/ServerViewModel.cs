using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class ServerViewModel
    {
        public List<string> Hosts { get; set; }
        public string CurrentHost { get; set; }
    }
}