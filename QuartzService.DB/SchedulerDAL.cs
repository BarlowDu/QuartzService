using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.DB
{
    public class SchedulerDAL
    {
        public DataTable GetAllScheduler()
        {
            string sql = "select * from Quartz_Scheduler where IsEnable=1";
            SqlConnection conn = new SqlConnection("data source=.;initial catalog=quartznet;user id=sa;password=123456;multipleactiveresultsets=True;");
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
