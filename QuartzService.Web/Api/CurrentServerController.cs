using Ionic.Zip;
using QuartzService.Web.App_Start;
using QuartzService.Web.Models;
using QuartzService.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace QuartzService.Web.Api
{
    [EnableCors("*", "*", "*")]
    public class CurrentServerController : ApiController
    {




        public IEnumerable<SchedulerModel> GetAllSchedulers()
        {
            return SchedulerManager.Instance.Schedulers;
        }

        [HttpGet]
        public IEnumerable<SchedulerModel> ReviseAll()
        {
            SchedulerManager.Instance.Revise();
            return SchedulerManager.Instance.Schedulers;
        }

        public SchedulerModel GetScheduler(int schId)
        {
            var scheduler = SchedulerManager.Instance.Schedulers.FirstOrDefault(t => t.SchedulerId == schId);
            return scheduler;
        }

        public CallbackModel StopScheduler(SchedulerWebApiModel sch)
        {
            try
            {
                SchedulerManager.Instance.ShutDownScheduler(sch.schId);
                return new CallbackModel(true);
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }

        [HttpPost]
        public CallbackModel StartScheduler(SchedulerWebApiModel sch)
        {
            try
            {
                SchedulerManager.Instance.StartScheduler(HostingEnvironment.ApplicationPhysicalPath, sch.schId);
                return new CallbackModel(true);
            }
            catch (Exception ex)
            {

                return new CallbackModel(false, ex.Message);
            }
        }

        [HttpPost]
        public CallbackModel UploadScheduler()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                var part = new MultipartFormDataConverter(Request.Content);
                int schId = int.Parse(part.FormData["SChId"]);
                SchedulerModel scheduler = SchedulerManager.Instance.Schedulers.FirstOrDefault(t => t.SchedulerId == schId);
                if (scheduler != null)
                {
                    string dir = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + scheduler.Directory);
                    string fileName = string.Format("{0}-{1:yyyyMMddHHmmss}.zip", scheduler.SchedulerName, DateTime.Now);
                    string fileFullName = Path.Combine(dir, fileName);
                    if (Directory.Exists(dir) == false)
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var file = part.FileContents["fileUpload"];
                    file.Save(fileFullName);
                    using (ZipFile zip1 = ZipFile.Read(fileFullName, new ReadOptions() { Encoding = Encoding.Default }))
                    {
                        foreach (ZipEntry e in zip1)
                        {
                            e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }

                }

                return new CallbackModel(true);
            }
            catch (Exception ex)
            {

                return new CallbackModel(false, ex.Message);
            }
        }

        public SchedulerModel Revise(SchedulerWebApiModel sch)
        {
            return SchedulerManager.Instance.ReviseScheduler(sch.schId);
        }

        [HttpPost]
        public CallbackModel KillProcess(SchedulerWebApiModel sch)
        {
            try
            {
                SchedulerManager.Instance.KillProcess(sch.schId);
                return new CallbackModel(true);
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }
    }
}
