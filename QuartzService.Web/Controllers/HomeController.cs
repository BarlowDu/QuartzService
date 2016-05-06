using Ionic.Zip;
using Quartz;
using Quartz.Impl;
using QuartzService.DB;
using QuartzService.DB.Model;
using QuartzService.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QuartzService.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }





        public ActionResult AllScheduler()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ReloadScheduler()
        {
            try
            {
                SchedulerManager.Instance.ReloadScheduler();
                return Json(new CallbackModel(true));
            }
            catch (Exception ex)
            {
                return Json(new CallbackModel(false, ex.Message));
            }
        }

        public ActionResult Get()
        {
            return Json(SchedulerManager.Instance.Schedulers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSchedulerData()
        {

            var dal = new SchedulerDAL();
            var result = dal.GetAllScheduler();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetScheduler(int schId)
        {
            var dal = new SchedulerDAL();
            var result = dal.GetScheduler(schId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult SaveScheduler(QuartzSchedulerModel model)
        {
            bool success = true;
            CallbackModel result;
            var dal = new SchedulerDAL();
            try
            {
                if (dal.CheckSchedulerNameExists(model.SchedulerName, model.SchedulerId))
                {
                    return Json(new CallbackModel(false, "SchedulerName必须唯一"));
                }
                if (model.SchedulerId <= 0)
                {
                    success = dal.AddScheduler(model);
                }
                else
                {
                    success = dal.UpdateScheduler(model);
                }
                string msg = "保存成功";
                if (success == false)
                {
                    msg = "保存失败";
                }
                return Json(new CallbackModel(success, msg));

            }
            catch (Exception ex)
            {
                return Json(new CallbackModel(success, ex.Message));
            }
        }

        public ActionResult GetAllServer()
        {
            ServerDAL dal = new ServerDAL();
            ServerViewModel result = new ServerViewModel();
            var servers = dal.GetAllServer();
            result.Hosts = servers.Select(t => t.ServerName).ToList();
            result.CurrentHost = ControllerContext.HttpContext.Request.Url.Authority;
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}