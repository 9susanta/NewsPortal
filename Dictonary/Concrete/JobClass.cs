using log4net;
using NewsPortal.DB;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NewsPortal.Concrete
{
    public class JobClass : IJob
    {
        ILog Log = log4net.LogManager.GetLogger(typeof(JobClass));
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Info("Job was getting called");
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                    {
                        var item = portalEntities.SchdulePostConfig.ToList();
                        foreach(var lvltem in item)
                        {
                            if(lvltem.ScheduleTime<=DateTime.Now)
                            {
                                try
                                {
                                    SqlCommand cmd = new SqlCommand("usp_AutoPublish", connection);
                                    cmd.Parameters.Add("@PostId", SqlDbType.Decimal).Value = lvltem.PostId;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 90;
                                    if (connection.State == ConnectionState.Closed)
                                        connection.Open();
                                    int i = cmd.ExecuteNonQuery();
                                    connection.Close();
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("Error Inside Loop",ex);
                                }

                            }
                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}