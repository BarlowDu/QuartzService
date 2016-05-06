using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class Summary
    {
        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("none")]
        public int None { get; set; }

        [JsonProperty("stop")]
        public int Stop { get; set; }

        [JsonProperty("running")]
        public int Running { get; set; }

        [JsonProperty("processRunning")]
        public int ProcessRunning { get; set; }
    }
}