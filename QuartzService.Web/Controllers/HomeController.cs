using Ionic.Zip;
using Quartz;
using Quartz.Impl;
using QuartzService.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
        public ActionResult ShutDown(int schId)
        {
            SchedulerManager.Instance.ShutDownScheduler(schId);

            return Json(new CallbackModel(true));
        }


        [HttpPost]
        public ActionResult StartScheduler(int schId)
        {
            SchedulerManager.Instance.StartScheduler(this.HttpContext, schId);
            return Json(new CallbackModel(true));
        }


        [HttpPost]
        public ActionResult Revise()
        {
            SchedulerManager.Instance.Revise();
            return Json(new CallbackModel(true));
        }

        [HttpPost]
        public ActionResult ReloadScheduler()
        {
            SchedulerManager.Instance.ReloadScheduler();
            return Json(new CallbackModel(true));
        }

        [HttpPost]
        public ActionResult KillProcess(int schId)
        {
            SchedulerManager.Instance.KillProcess(schId);
            return Json(new CallbackModel(true));
        }

        [HttpPost]
        public ActionResult UploadApplication(int schId)
        {
            var file = Request.Files[0];
            SchedulerModel scheduler = SchedulerManager.Instance.Schedulers.FirstOrDefault(t => t.SchedulerId == schId);
            if (scheduler != null)
            {
                string dir = HttpContext.Server.MapPath("~/" + scheduler.Directory);
                string fileName = string.Format("{0}-{1:yyyyMMddHHmmss}.zip", scheduler.SchedulerName, DateTime.Now);
                string fileFullName = Path.Combine(dir, fileName);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
                byte[] buffer = new byte[file.InputStream.Length];
                file.InputStream.Read(buffer, 0, buffer.Length);

                using (var fs = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }

                using (ZipFile zip1 = ZipFile.Read(fileFullName))
                {
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

            }
            return Json(new CallbackModel(true));
        }

        [HttpPost]
        public ActionResult PauseJob(int schId, string groupName, string jobName)
        {
            SchedulerManager.Instance.PauseJob(schId, groupName, jobName);
            return Json(new CallbackModel(true));
        }

        [HttpPost]
        public ActionResult ResumeJob(int schId, string groupName, string jobName)
        {
            SchedulerManager.Instance.ResumeJob(schId, groupName, jobName);
            return Json(new CallbackModel(true));
        }
    }
}