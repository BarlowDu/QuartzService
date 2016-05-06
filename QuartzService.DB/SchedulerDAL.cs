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
    public class SchedulerDAL
    {

        public IEnumerable<QuartzSchedulerModel> GetAllScheduler()
        {
            string sql = "select * from Quartz_Scheduler where IsEnable=1";
            SqlConnection conn = new SqlConnection(DBConnection.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return OrmTransfer.DataTableToList<QuartzSchedulerModel>(dt);
        }

        public bool AddScheduler(QuartzSchedulerModel model)
        {
            int count = 0;
            string sql = @"INSERT INTO Quartz_Scheduler( SchedulerName ,Directory ,FileName ,Port)
VALUES  (  @SchedulerName ,@Directory ,@FileName ,@Port )";
            SqlParameter[] ps = new SqlParameter[] { 
            new SqlParameter("@SchedulerName",model.SchedulerName),
            new SqlParameter("@Directory",model.Directory),
            new SqlParameter("@FileName",model.FileName),
            new SqlParameter("@Port",model.Port),
            new SqlParameter("@IsEnable",model.IsEnable)
            };
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                foreach (var p in ps)
                {
                    cmd.Parameters.Add(p);
                }
                conn.Open();
                count = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return count > 0;
        }

        public bool UpdateScheduler(QuartzSchedulerModel model)
        {
            int count = 0;
            string sql = @"UPDATE Quartz_Scheduler SET SchedulerName=@SchedulerName ,Directory=@Directory ,FileName=@FileName ,Port=@Port 
                        WHERE SchedulerId=@SchedulerId";
            SqlParameter[] ps = new SqlParameter[] { 
            new SqlParameter("@SchedulerId",model.SchedulerId),
            new SqlParameter("@SchedulerName",model.SchedulerName),
            new SqlParameter("@Directory",model.Directory),
            new SqlParameter("@FileName",model.FileName),
            new SqlParameter("@Port",model.Port)
            };
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                foreach (var p in ps)
                {
                    cmd.Parameters.Add(p);
                }
                conn.Open();
                count = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return count > 0;
        }

        public QuartzSchedulerModel GetScheduler(int schId)
        {
            string sql = @"SELECT * FROM Quartz_Scheduler WHERE SchedulerId=@SchedulerId";

            SqlConnection conn = new SqlConnection(DBConnection.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            da.SelectCommand.Parameters.Add(new SqlParameter("@SchedulerId", schId));
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return OrmTransfer.GetModel<QuartzSchedulerModel>(dt.Rows[0]);
        }

        public bool CheckSchedulerNameExists(string schName, int schId)
        {
            string sql = "select count(1) FROM Quartz_Scheduler WHERE SchedulerName=@SchedulerName AND SchedulerId!=@SchedulerId";
            SqlConnection conn = new SqlConnection(DBConnection.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            da.SelectCommand.Parameters.Add(new SqlParameter("@SchedulerId", schId));
            da.SelectCommand.Parameters.Add(new SqlParameter("@SchedulerName", schName));
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return true;
            }
            var count = Convert.ToInt32(dt.Rows[0][0]);
            return count > 0;
        }
    }
}
