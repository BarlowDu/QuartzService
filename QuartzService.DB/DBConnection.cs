using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.DB
{
    internal class DBConnection
    {
        public static string ConnectionString { get; private set; }
        static DBConnection()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }
    }
}
