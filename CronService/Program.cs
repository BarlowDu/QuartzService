using QuartzService.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            LoadAssemblies();

            var section = ServiceSection.GetSection("quartzservice");
            var invoker = section.GetServerInvoker();
            if (invoker != null)
            {
                invoker.Method.Invoke(invoker.Instance,null);
            }

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
