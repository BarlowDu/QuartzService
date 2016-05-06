using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class SchedulerCollection
    {
        [JsonProperty("schedulerId")]
        public int SchedulerId { get; set; }

        [JsonProperty("list")]
        public Dictionary<string, SchedulerModel> List { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }
    }
}