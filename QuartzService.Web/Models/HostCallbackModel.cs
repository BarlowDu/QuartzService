using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class HostCallbackModel : CallbackModel
    {
        public string host { get; set; }
        public HostCallbackModel()
        { }
        public HostCallbackModel(bool _result, string _msg, string _host)
        {
            result = _result;
            msg = _msg;
            host = _host;
        }



    }
}