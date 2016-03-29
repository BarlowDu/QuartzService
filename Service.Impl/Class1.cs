using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class Class1
    {
        public void Run() {
            Console.WriteLine("{0}\t{1:yyyy-MM-dd HH:mm:ss}", this.GetType().Assembly.FullName, DateTime.Now);
        }
    }
}
