using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class SchedulerWebApiModel
    {
        //[JsonProperty("schId")]
        public int schId { get; set; }

        //[JsonProperty("host")]
        public string host { get; set; }
    }
}