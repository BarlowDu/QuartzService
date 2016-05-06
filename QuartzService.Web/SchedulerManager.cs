using Quartz;
using Quartz.Impl;
using QuartzService.DB;
using QuartzService.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;

namespace QuartzService.Web
{
    public class SchedulerManager
    {
        static SchedulerManager _instance = new SchedulerManager();
        public static SchedulerManager Instance
        {
            get { return _instance; }
        }
        List<string> hosts = new List<string>();
        List<SchedulerModel> schedulers = new List<SchedulerModel>();
        object objLock = new object();

        public List<SchedulerModel> Schedulers
        {
            get { return schedulers; }
        }
        public IEnumerable<string> Hosts
        {
            get { return hosts; }
        }

        public void RefreshHost()
        {
            lock (objLock)
            {

                var dal = new ServerDAL();
                var hosts = dal.GetAllServer();
                this.hosts = hosts.Select(t => t.ServerName).ToList();
            }
        }
        public void InitSchedulers()
        {
            RefreshHosts();
            lock (objLock)
            {
                schedulers.AddRange(GetSchedulersFromDB());
            }
            Revise();
        }

        public void RefreshHosts()
        {
            ServerDAL sal = new ServerDAL();
            var result = sal.GetAllServer();
            hosts = result.Select(t => t.ServerName).ToList();
        }
        public void StartScheduler(string rootPath, int schId)
        {
            lock (objLock)
            {
                SchedulerModel scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
                if (scheduler != null)
                {
                    Process[] ps = Process.GetProcessesByName(scheduler.ProcessName);
                    if (ps != null && ps.Length > 0)
                    {
                        throw new Exception("已经存在进程" + scheduler.ProcessName);
                    }
                    Process process = new Process();
                    ProcessStartInfo info = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(rootPath, scheduler.Directory, scheduler.FileName),
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false
                    };
                    process.StartInfo = info;
                    process.Start();

                    BindScheduler(scheduler, process);
                    scheduler.Status = SchedulerStatus.Running;
                }
            }
        }


        public void ShutDownScheduler(int schId)
        {
            lock (objLock)
            {
                SchedulerModel scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
                if (scheduler != null && scheduler.Scheduler != null && scheduler.Scheduler.IsShutdown == false)
                {
                    scheduler.Scheduler.Shutdown(true);
                    scheduler.Scheduler = null;
                    scheduler.Jobs = null;
                    scheduler.Process = null;
                    scheduler.Status = SchedulerStatus.Stop;
                }
            }
        }



        /// <summary>
        /// 还要不要?????
        /// </summary>
        /// <returns></returns>
        public object GetSchedulerData()
        {
            throw new NotImplementedException();
        }

        public void KillProcess(int schId)
        {
            SchedulerModel scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);

            Process[] ps = Process.GetProcessesByName(scheduler.ProcessName);
            foreach (var p in ps)
            {
                p.Kill();
            }
        }

        public void ReloadScheduler()
        {
            lock (objLock)
            {
                schedulers = GetSchedulersFromDB();
            }
            Revise();
        }

        /// <summary>
        /// 校验
        /// </summary>
        public void Revise()
        {
            lock (objLock)
            {
                ServerDAL dal = new ServerDAL();
                hosts = dal.GetAllServer().Select(t => t.ServerName).ToList();
                schedulers = GetSchedulersFromDB();
                foreach (var scheduler in schedulers)
                {
                    ReviseScheduler(scheduler);
                }
            }
        }

        public SchedulerModel ReviseScheduler(int schId)
        {
            var scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
            if (scheduler != null)
            {
                return ReviseScheduler(scheduler);
            }
            else
            {

                throw new Exception("不存在Scheduler");
            }
        }

        private SchedulerModel ReviseScheduler(SchedulerModel scheduler)
        {
            if (string.IsNullOrEmpty(scheduler.ProcessName))
            {
                ClearScheduler(scheduler);
            }
            else
            {
                Process[] ps = Process.GetProcessesByName(scheduler.ProcessName);
                if (ps != null && ps.Length > 0)
                {
                    try
                    {
                        if (CheckSchedulerExists(scheduler) == false)
                        {
                            ClearScheduler(scheduler);
                            scheduler.Status = SchedulerStatus.ProcessRunning;
                        }
                        else
                        {
                            BindScheduler(scheduler, ps[0]);
                            scheduler.Status = SchedulerStatus.Running;
                        }
                    }
                    catch (RemotingException ex)
                    {
                        throw ex;///TODO?????
                    }
                }
                else
                {
                    ClearScheduler(scheduler);
                    scheduler.Status = SchedulerStatus.Stop;
                }
            }
            return scheduler;

        }

        private bool CheckSchedulerExists(SchedulerModel scheduler)
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = string.Format("tcp://localhost:{0}/QuartzScheduler", scheduler.Port);
            ISchedulerFactory factory = new StdSchedulerFactory(properties);

            try
            {
                if (SchedulerRepository.Instance.Lookup(scheduler.SchedulerName) == null)
                {
                    scheduler.Scheduler = factory.GetScheduler();
                }
                else
                {
                    scheduler.Scheduler = factory.GetScheduler(scheduler.SchedulerName);
                }
                if (scheduler.Scheduler.IsShutdown == true)
                {
                    return false;
                }
                return true;
            }
            catch (RemotingException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void BindScheduler(SchedulerModel scheduler, Process process)
        {
            scheduler.Process = process;
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = string.Format("tcp://localhost:{0}/QuartzScheduler", scheduler.Port);
            ISchedulerFactory factory = new StdSchedulerFactory(properties);

            try
            {
                if (SchedulerRepository.Instance.Lookup(scheduler.SchedulerName) == null)
                {
                    scheduler.Scheduler = factory.GetScheduler();
                }
                else
                {
                    scheduler.Scheduler = factory.GetScheduler(scheduler.SchedulerName);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            scheduler.Jobs = new List<JobModel>();
            var groups = scheduler.Scheduler.GetJobGroupNames();
            foreach (var group in groups)
            {
                var jobkeys = scheduler.Scheduler.GetJobKeys(Quartz.Impl.Matchers.GroupMatcher<JobKey>.GroupContains(group));
                foreach (var key in jobkeys)
                {
                    var job = new JobModel() { GroupName = key.Group, JobName = key.Name };
                    scheduler.Jobs.Add(job);
                    BindJobStatus(scheduler.Scheduler, job);
                }
            }
        }

        private void BindJobStatus(IScheduler scheduler, JobModel job)
        {
            if (scheduler == null || job == null)
            {
                return;
            }
            var triggers = scheduler.GetTriggersOfJob(new JobKey(job.JobName, job.GroupName));
            if (triggers != null && triggers.Count > 0)
            {
                job.Status = scheduler.GetTriggerState(triggers[0].Key);
            }
        }

        private void ClearScheduler(SchedulerModel scheduler)
        {
            //scheduler.ProcessName = null;
            scheduler.Scheduler = null;
            scheduler.Process = null;
            scheduler.Jobs = null;
        }

        /// <summary>
        /// 暂停Job
        /// </summary>
        public void PauseJob(int schId, string groupName, string jobName)
        {
            lock (objLock)
            {
                var scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
                if (scheduler != null)
                {
                    scheduler.Scheduler.PauseJob(new JobKey(jobName, groupName));
                    var job = scheduler.Jobs.FirstOrDefault(t => t.GroupName == groupName && t.JobName == jobName);
                    BindJobStatus(scheduler.Scheduler, job);
                }

            }
        }

        /// <summary>
        /// 重启Job
        /// </summary>
        public void ResumeJob(int schId, string groupName, string jobName)
        {
            lock (objLock)
            {
                var scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
                if (scheduler != null)
                {
                    scheduler.Scheduler.ResumeJob(new JobKey(jobName, groupName));
                    var job = scheduler.Jobs.FirstOrDefault(t => t.GroupName == groupName && t.JobName == jobName);
                    BindJobStatus(scheduler.Scheduler, job);
                }
            }
        }

        private List<SchedulerModel> GetSchedulersFromDB()
        {

            List<SchedulerModel> result = new List<SchedulerModel>();
            SchedulerDAL dal = new SchedulerDAL();
            var schs = dal.GetAllScheduler();
            foreach (var sch in schs)
            {
                SchedulerModel item = new SchedulerModel()
                {
                    SchedulerId = sch.SchedulerId,
                    SchedulerName = sch.SchedulerName,
                    Directory = sch.Directory,
                    FileName = sch.FileName,
                    Port = sch.Port,
                    IsEnable = sch.IsEnable
                };
                result.Add(item);
            }
            return result;
        }

        public SchedulerStatus GetSchedulerStatus(int schId)
        {
            SchedulerStatus result = SchedulerStatus.None;
            var scheduler = schedulers.FirstOrDefault(t => t.SchedulerId == schId);
            if (scheduler != null)
            {
                result = scheduler.Status;
            }
            return result;

        }

    }


}