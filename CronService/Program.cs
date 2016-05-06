using QuartzService.Config;
using QuartzService.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace CronService
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Console.Title = Process.GetCurrentProcess().ProcessName;

            LoadAssemblies();

            var section = ServiceSection.GetSection("quartzservice");
            var invoker = section.GetServerInvoker();
            if (invoker == null)
            {

                throw new NullReferenceException("scheduler服务实例获取失败.");
            }
            invoker.Method.Invoke(invoker.Instance, null);

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            LogHandler.Error("全局异常", ex);

        }

        private static void LoadAssemblies()
        {

            DirectoryInfo currentDir = new FileInfo(typeof(Program).Assembly.Location).Directory;
            var files = currentDir.GetFiles("*.dll");
            foreach (var file in files)
            {
                Assembly.LoadFile(file.FullName);
            }

            files = currentDir.GetFiles("*.exe");
            foreach (var file in files)
            {
                Assembly.LoadFile(file.FullName);
            }
        }
    }




}
