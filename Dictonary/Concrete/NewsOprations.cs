using NewsPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.DB;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Caching;
using log4net;
using System.Threading;

namespace NewsPortal.Concrete
{
    public class NewsOprations:IDisposable
    {
        ILog Log = log4net.LogManager.GetLogger(typeof(Usermanament));
        public decimal NewsPost(ClsPost objclsPost)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    NewsPost newsPost = new NewsPost();
                    newsPost.EnglishTitle = objclsPost.EnglishTitle;
                    newsPost.OdiaTitle = objclsPost.OdiaTitle;
                    newsPost.EngShortDesc = objclsPost.EngShortDesc;
                    newsPost.ODShortDesc = objclsPost.ODShortDesc;
                    newsPost.SeoMeta = objclsPost.SeoMeta;
                    newsPost.Tags = objclsPost.Tags;
                    newsPost.CategoryId = objclsPost.CategoryId;
                    newsPost.HeaderImageName = objclsPost.ImageName;
                    newsPost.ODContent = objclsPost.ODContent;
                    newsPost.IsActive = true;
                    newsPost.IsDeleted = false;
                    newsPost.IsReviewed = objclsPost.IsReviewed;
                    newsPost.CreatedBy = objclsPost.CreatedBy;
                    newsPost.ReviewedBy = objclsPost.ReviewedBy;
                    newsPost.PostedDate = objclsPost.PostedDate;
                    newsPost.PostedOn = objclsPost.PostedOn;
                    newsPost.PostedMonth = objclsPost.PostedMonth;
                    newsPost.PostedYear = objclsPost.PostedYear;
                    newsPost.SlugUrl = objclsPost.SlugUrl;
                    newsPost.Thumbnail86 = objclsPost.ImageName.Replace("Img", "Thumbnail_86x64");
                    newsPost.Thumbnail210 = objclsPost.ImageName.Replace("Img", "Thumbnail_210x136");
                    newsPost.Thumbnail279 = objclsPost.ImageName.Replace("Img", "Thumbnail_279x220");
                    newsPost.Frequency = 0;
                    portalEntities.NewsPosts.Add(newsPost);
                    portalEntities.SaveChanges();
                    return newsPost.Id;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return 0;
        }

        public DataSet GetAllNews(decimal NewsId, int CreateId, int ReviewId, int PageIndex, DateTime? Stardt, DateTime? Enddt)
        {
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_GetAllNews", connection);
                    cmd.Parameters.Add("@NewsId", SqlDbType.Int).Value = NewsId;
                    cmd.Parameters.Add("@CreatedUserId", SqlDbType.Int).Value = CreateId;
                    cmd.Parameters.Add("@ReviewdBy", SqlDbType.Int).Value = ReviewId;
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;
                    cmd.Parameters.Add("@StartDt", SqlDbType.DateTime).Value = Stardt;
                    cmd.Parameters.Add("@EndDt", SqlDbType.DateTime).Value = Enddt;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;
                    try
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                        connection.Close();
                    }

                }
            }
            return null;
        }
        public DataSet GetNews(int pageIndex, int pageSize, int catagoryId)
        {
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_GetClientSideNews", connection);
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex + 1;
                    cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                    cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = catagoryId;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;
                    try
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                        connection.Close();
                    }

                }
            }
            return null;
        }
        public DataTable GetSectionData(string SectionName, int PageIndex = 0, int catagoryId = 0)
        {
            NewsList newsList = new NewsList();
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_GetSetionData", connection);
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex + 1;
                    cmd.Parameters.Add("@SectionName", SqlDbType.NVarChar).Value = SectionName;
                    cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = catagoryId;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;
                    try
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }

                }
            }
            return null;
        }

        public DataSet GetClientCategory(int CategoryId)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                    {
                        var cmd = new SqlCommand("usp_GetCatagoryData", connection);
                        cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 90;
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            return dataSet;
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

            }
            return null;
        }
        public DataSet GetNewPost(decimal Id,int type)
        {
            try
            {
                var np = new NewsOprations();
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                    {
                        var cmd = new SqlCommand("usp_GetNewPost", connection);
                        cmd.Parameters.Add("@Id", SqlDbType.Decimal).Value = Id;
                        cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 90;
                        using (var dataAdapter = new SqlDataAdapter(cmd))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            Task.Factory.StartNew(() => np.UpdateNewsCount(Id));
                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

            }
            return null;
        }
        public DataSet GetNewsBySearch(int pageIndex, string catagory)
        {
            var listPst = new List<ClsPost>();
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    var cmd = new SqlCommand("usp_GetSearch", connection);
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
                    cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = 10;
                    cmd.Parameters.Add("@SearchStr", SqlDbType.NVarChar).Value = catagory;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;
                    try
                    {
                        using (var dataAdapter = new SqlDataAdapter(cmd))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            return dataSet;
                        };
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                    return null;
                }
            }
        }

        public DataSet GetHomePage()
        {
            var HomeDs = System.Web.HttpRuntime.Cache["HomeData"] as DataSet;
            if(HomeDs != null)
            {
                return HomeDs;
            }
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    var cmd = new SqlCommand("usp_GetIndexPage", connection);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 90;
                        connection.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var ds = new DataSet();
                            da.Fill(ds);
                            System.Web.HttpRuntime.Cache.Insert("HomeData", ds, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                            return ds;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                    return null;
                }
            }

        }
        public void UpdateNewsCount(decimal NewsId)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("usp_UpdateNewsCount", connection);
                        cmd.Parameters.Add("@NewsId", SqlDbType.Decimal).Value = NewsId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 90;
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        int i = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public DataSet GetAnalytics(int userId)
        {
            using (NewsPortalEntities portalEntities = new NewsPortalEntities())
            {
                using (SqlConnection connection = new SqlConnection(portalEntities.Database.Connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_GetAnalytics", connection);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;
                    try
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet dt = new DataSet();
                        da.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }

                }
            }
            return null;
        }
        public clsUser getCurrentUserId(int userId)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var item = portalEntities.Database.SqlQuery<clsUser>("usp_GetUser @UserId", new SqlParameter("@UserId", userId)).FirstOrDefault();
                    return item;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        public void UpdateData()
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var item = portalEntities.Database.SqlQuery<int>("usp_LatestPopular");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~NewsOprations() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}