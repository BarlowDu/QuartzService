using QuartzService.DB;
using QuartzService.Web.Models;
using QuartzService.Web.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace QuartzService.Web.Api
{
    public class AllServerController : ApiController
    {
        public SchedulerDictionary GetAllSchedulers()
        {
            SchedulerDictionary result = new SchedulerDictionary();

            var dal = new SchedulerDAL();
            var schedulers = dal.GetAllScheduler();
            foreach (var s in schedulers)
            {
                result.Add(s.SchedulerName, new SchedulerCollection()
                {
                    SchedulerId = s.SchedulerId,
                    List = new Dictionary<string, SchedulerModel>(),
                    Summary = new Summary()
                });
            }
            Parallel.ForEach(SchedulerManager.Instance.Hosts, host =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    var response = client.GetAsync("http://" + host + "/api/CurrentServer/GetAllSchedulers").Result;
                    var list = response.Content.ReadAsAsync<List<SchedulerModel>>().Result;
                    foreach (var s in list)
                    {
                        result[s.SchedulerName].List.Add(host, s);
                    }
                }
                catch (Exception ex)
                {
                    foreach (var kv in result)
                    {
                        kv.Value.List.Add(host, null);
                    }
                }
            });

            foreach (var kv in result)
            {
                var item = kv.Value;
                foreach (var s in item.List)
                {
                    if (s.Value == null)
                    {
                        item.Summary.Error++;
                        continue;
                    }
                    switch (s.Value.Status)
                    {
                        case SchedulerStatus.None:
                            item.Summary.None++;
                            break;
                        case SchedulerStatus.Stop:
                            item.Summary.Stop++;
                            break;
                        case SchedulerStatus.Running:
                            item.Summary.Running++;
                            break;
                        case SchedulerStatus.ProcessRunning:
                            item.Summary.ProcessRunning++;
                            break;
                        default:

                            break;
                    }

                }
            }
            return result;
        }

        [HttpGet]
        public SchedulerDictionary ReviseAll()
        {
            SchedulerDictionary result = new SchedulerDictionary();

            SchedulerManager.Instance.RefreshHosts();
            var dal = new SchedulerDAL();
            var schedulers = dal.GetAllScheduler();
            foreach (var s in schedulers)
            {
                result.Add(s.SchedulerName, new SchedulerCollection()
                {
                    SchedulerId = s.SchedulerId,
                    List = new Dictionary<string, SchedulerModel>(),
                    Summary = new Summary()
                });
            }
            Parallel.ForEach(SchedulerManager.Instance.Hosts, host =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    var response = client.GetAsync("http://" + host + "/api/CurrentServer/ReviseAll").Result;
                    var list = response.Content.ReadAsAsync<List<SchedulerModel>>().Result;
                    foreach (var s in list)
                    {
                        result[s.SchedulerName].List.Add(host, s);
                    }
                }
                catch (Exception ex)
                {
                    foreach (var kv in result)
                    {
                        kv.Value.List.Add(host, null);
                    }
                }
            });

            foreach (var kv in result)
            {
                var item = kv.Value;
                foreach (var s in item.List)
                {
                    if (s.Value == null)
                    {
                        item.Summary.Error++;
                        continue;
                    }
                    switch (s.Value.Status)
                    {
                        case SchedulerStatus.None:
                            item.Summary.None++;
                            break;
                        case SchedulerStatus.Stop:
                            item.Summary.Stop++;
                            break;
                        case SchedulerStatus.Running:
                            item.Summary.Running++;
                            break;
                        case SchedulerStatus.ProcessRunning:
                            item.Summary.ProcessRunning++;
                            break;
                        default:

                            break;
                    }

                }
            }
            return result;
        }

        [HttpPost]
        public SchedulerCollection Revise(SchedulerWebApiModel sch)
        {
            SchedulerCollection result = new SchedulerCollection()
            {
                SchedulerId = sch.schId,
                List = new Dictionary<string, SchedulerModel>(),
                Summary = new Summary()
            };
            Parallel.ForEach(SchedulerManager.Instance.Hosts, (host) =>
           {

               HttpClient client = new HttpClient();
               try
               {
                   var response = client.PostAsync<SchedulerWebApiModel>("http://" + host + "/api/CurrentServer/Revise",
                       sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                   var item = response.Content.ReadAsAsync<SchedulerModel>().Result;

                   result.List.Add(host, item);
               }
               catch (Exception ex)
               {
                   result.List.Add(host, null);
               }
           });
            foreach (var s in result.List)
            {

                if (s.Value == null)
                {
                    result.Summary.Error++;
                    continue;
                }
                switch (s.Value.Status)
                {
                    case SchedulerStatus.None:
                        result.Summary.None++;
                        break;
                    case SchedulerStatus.Stop:
                        result.Summary.Stop++;
                        break;
                    case SchedulerStatus.Running:
                        result.Summary.Running++;
                        break;
                    case SchedulerStatus.ProcessRunning:
                        result.Summary.ProcessRunning++;
                        break;
                    default:

                        break;
                }
            }

            return result;
        }

        public CallbackModel StartHostScheduler(SchedulerWebApiModel sch)
        {
            HttpClient client = new HttpClient();
            try
            {
                var response = client.PostAsync<SchedulerWebApiModel>("http://" + sch.host + "/api/CurrentServer/StartScheduler",
                           sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                var result = response.Content.ReadAsAsync<CallbackModel>().Result;
                return result;
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }

        public CallbackModel StopHostScheduler(SchedulerWebApiModel sch)
        {
            HttpClient client = new HttpClient();
            try
            {
                var response = client.PostAsync<SchedulerWebApiModel>("http://" + sch.host + "/api/CurrentServer/StopScheduler",
                           sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                var result = response.Content.ReadAsAsync<CallbackModel>().Result;
                return result;
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }

        public CallbackModel KillProcess(SchedulerWebApiModel sch)
        {

            HttpClient client = new HttpClient();
            try
            {
                var response = client.PostAsync<SchedulerWebApiModel>("http://" + sch.host + "/api/CurrentServer/KillProcess",
                           sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                var result = response.Content.ReadAsAsync<CallbackModel>().Result;
                return result;
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }

        public SchedulerModel ReviseHostScheduler(SchedulerWebApiModel sch)
        {
            HttpClient client = new HttpClient();
            try
            {
                var response = client.PostAsync<SchedulerWebApiModel>("http://" + sch.host + "/api/CurrentServer/Revise",
                           sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                var result = response.Content.ReadAsAsync<SchedulerModel>().Result;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public CallbackModel UploadHostScheduler(string host)
        {
            HttpClient client = new HttpClient();
            try
            {

                var response = client.PostAsync("http://" + host + "/api/CurrentServer/UploadScheduler", Request.Content).Result;
                var result = response.Content.ReadAsAsync<CallbackModel>().Result;
                return result;
            }
            catch (Exception ex)
            {
                return new CallbackModel(false, ex.Message);
            }
        }
        ///////////////////////////////////////////////


        public List<HostCallbackModel> StartAllHostScheduler(SchedulerWebApiModel sch)
        {
            List<HostCallbackModel> result = new List<HostCallbackModel>();
            Parallel.ForEach(SchedulerManager.Instance.Hosts, (host) =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    var response = client.PostAsync<SchedulerWebApiModel>("http://" + host + "/api/CurrentServer/StartScheduler",
                               sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                    var callback = response.Content.ReadAsAsync<CallbackModel>().Result;
                    result.Add(new HostCallbackModel(callback.result, callback.msg, host));
                }
                catch (Exception ex)
                {
                    result.Add(new HostCallbackModel(false, ex.Message, host));
                }

            });
            return result;
        }


        public List<HostCallbackModel> StopAllHostScheduler(SchedulerWebApiModel sch)
        {
            List<HostCallbackModel> result = new List<HostCallbackModel>();
            Parallel.ForEach(SchedulerManager.Instance.Hosts, (host) =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    var response = client.PostAsync<SchedulerWebApiModel>("http://" + host + "/api/CurrentServer/StopScheduler",
                               sch, new System.Net.Http.Formatting.JsonMediaTypeFormatter()).Result;
                    var callback = response.Content.ReadAsAsync<CallbackModel>().Result;
                    result.Add(new HostCallbackModel(callback.result, callback.msg, host));
                }
                catch (Exception ex)
                {
                    result.Add(new HostCallbackModel(false, ex.Message, host));
                }

            });
            return result;
        }
        [HttpPost]
        public List<HostCallbackModel> UploadAllScheduler()
        {
            //System.Web.Script.Serialization.JavaScriptSerializer 
            List<HostCallbackModel> result = new List<HostCallbackModel>();
            Parallel.ForEach(SchedulerManager.Instance.Hosts, (host) =>
            {
                HttpClient client = new HttpClient();
                try
                {

                    var response = client.PostAsync("http://" + host + "/api/CurrentServer/UploadScheduler", Request.Content).Result;
                    var callback = response.Content.ReadAsAsync<CallbackModel>().Result;
                    result.Add(new HostCallbackModel(callback.result, callback.msg, host));
                }
                catch (Exception ex)
                {
                    result.Add(new HostCallbackModel(false, ex.Message, host));
                }

            });
            return result;
        }
    }
}
