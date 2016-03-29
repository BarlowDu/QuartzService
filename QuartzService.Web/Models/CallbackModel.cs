using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Models
{
    public class CallbackModel
    {
        public CallbackModel()
        { }
        public CallbackModel(bool _result)
        {
            result = _result;
        }

        public CallbackModel(bool _result, string _msg)
        {
            result = _result;
            msg = _msg;
        }
        public bool result { get; set; }
        public string msg { get; set; }
    }
}