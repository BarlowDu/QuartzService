using QuartzService.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuartzService.Web.App_Start
{
    public class CustomerExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {

            QuartzService.Web.Log.LogHandler.Error("", filterContext.Exception);
            filterContext.Result = new JsonResult() { Data = new CallbackModel(false, filterContext.Exception.Message) };
            filterContext.ExceptionHandled = true;
        }
    }
}