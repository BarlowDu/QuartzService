using QuartzService.DB.Model;
using QuartzService.DB.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.DB
{
    public class ServerDAL
    {
        public IEnumerable<QuartzServerModel> GetAllServer()
        {
            string sql = "select * from Quartz_Server";
            SqlConnection conn = new SqlConnection(DBConnection.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return OrmTransfer.DataTableToList<QuartzServerModel>(dt);
        }
    }
}
