using log4net;
using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class NewsOprationsController : BaseController
    {
        private IOperation<NewsPost> newsPost = null;
        ILog Log = log4net.LogManager.GetLogger(typeof(NewsOprationsController));
        private IOperation<tblNewsType> newsType = null;
        private IOperation<SchdulePostConfig> schulepostconfig = null;
        public NewsOprationsController()
        {
            this.newsPost = new OpertionClass<NewsPost>();
            this.newsType = new OpertionClass<tblNewsType>();
            this.schulepostconfig = new OpertionClass<SchdulePostConfig>();
        }
        public NewsOprationsController(IOperation<NewsPost> _newsPost)
        {
            this.newsPost = _newsPost;
        }
        // GET: NewsOprations
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        [AuthorizationPrivilege]
        public ActionResult Dashbord()
        {
            return View();
        }
        [AuthorizationPrivilege]
        public ActionResult Analytics()
        {
            return View();
        }
        [HttpPost]
        public ContentResult HeaderImageUpload()
        {
            NewsPost nps = new NewsPost();
            try
            {
                int NewsId = 0;
                string FolderName = "";
                string path = "";
                if (Request.Params["newsID"] == "0")
                {
                    using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                    {
                        NewsId = portalEntities.NewsPosts.Where(x => x.PostedDate.Value.Year == DateTime.Now.Year && x.PostedDate.Value.Month == DateTime.Now.Month && x.PostedDate.Value.Day == DateTime.Now.Day).ToList().Count;
                    }
                    NewsId = NewsId + 1;

                    FolderName = DateTime.Now.Date.ToString("ddMMyyyy");
                    path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                    {
                        decimal newsID = Convert.ToDecimal(Request.Params["newsID"]);
                        nps = portalEntities.NewsPosts.Where(x => x.Id == newsID).FirstOrDefault();
                        string ImageName = "";
                        if(nps!=null)
                        {
                            DeleteImage(nps.HeaderImageName);
                            ImageName = nps.HeaderImageName;
                            FolderName = ImageName.Split('/')[0];
                            NewsId = int.Parse(ImageName.Split('/')[1]);
                        }
                    }
                    path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                string ImageUrl = "";
                string filename = "";
                string imgId = DateTime.Now.Hour + "" + DateTime.Now.Minute;
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase postedFile = Request.Files[key];
                    ImageUrl = Path.Combine(path, "Img_" + imgId + ".jpg");
                    Stream strm = postedFile.InputStream;
                    int flstatus = GenerateThumbnails(0.2, strm, ImageUrl, 400, 210);
                    filename = postedFile.FileName;
                    CreateThumbnail(postedFile, NewsId, FolderName, imgId);
                    CreateThumbnail279(postedFile, NewsId, FolderName, imgId);
                    CreateThumbnail210(postedFile, NewsId, FolderName, imgId);
                }
                if (Request.Params["newsID"] != "0")
                {
                    string Image = FolderName + "/" + NewsId + "/" + "Img_" + imgId + ".jpg";
                    using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                    {
                        decimal newsID = Convert.ToDecimal(Request.Params["newsID"]);
                        NewsPost newsPost = portalEntities.NewsPosts.Where(x => x.Id == newsID).FirstOrDefault();
                        newsPost.HeaderImageName = Image;
                        newsPost.Thumbnail86 = Image.Replace("Img", "Thumbnail_86x64");
                        newsPost.Thumbnail210 = Image.Replace("Img", "Thumbnail_210x136");
                        newsPost.Thumbnail279 = Image.Replace("Img", "Thumbnail_279x220");
                        portalEntities.NewsPosts.Attach(newsPost);
                        portalEntities.Entry(newsPost).State = EntityState.Modified;
                        portalEntities.SaveChanges();
                    }
                }
                return Content(FolderName + "/" + NewsId + "/" + "Img_" + imgId + ".jpg");
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        private void CreateThumbnail(HttpPostedFileBase postedFile,int NewsId, string FolderName,string imgId)
        {
            string path = Server.MapPath("~/Uploads/" + FolderName+"/"+ NewsId);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string ImageUrl = Path.Combine(path, "Thumbnail_86x64_" + imgId + ".jpg");
            Stream strm = postedFile.InputStream;
            int flstatus = GenerateThumbnails(0.1, strm, ImageUrl, 86, 64);
        }
        private void CreateThumbnail279(HttpPostedFileBase postedFile, int NewsId, string FolderName, string imgId)
        {
            string path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string ImageUrl = Path.Combine(path, "Thumbnail_279x220_" + imgId + ".jpg");
            Stream strm = postedFile.InputStream;
            int flstatus = GenerateThumbnails(0.1, strm, ImageUrl, 240, 130);
        }
        private void CreateThumbnail210(HttpPostedFileBase postedFile, int NewsId, string FolderName,string imgId)
        {
            string path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string ImageUrl = Path.Combine(path, "Thumbnail_210x136_" + imgId + ".jpg");
            Stream strm = postedFile.InputStream;
            int flstatus = GenerateThumbnails(0.1, strm, ImageUrl, 160, 103);
        }
        private int GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath,int newWidth,int newHeight)
        {
            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    thumbnailImg.Save(targetPath, image.RawFormat);
                    thumbGraph.Dispose();
                    thumbnailImg.Dispose();
                    image.Dispose();
                    return 1;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return 0;
        }

        [HttpPost]
        public JsonResult NewsPost(ClsPost clsPost)
        {
            try
            {
                var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                var Role = currentUser.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                clsPost.IsReviewed = false;
                clsPost.CreatedBy = int.Parse(UserId);
                if (clsPost.IsSchedule == false)
                {
                    if (Role == "SuperAdmin" || Role == "Admin")
                    {
                        clsPost.ReviewedBy = int.Parse(UserId);
                        clsPost.IsReviewed = true;
                    }
                }
                clsPost.PostedOn = DateTime.Now;
                clsPost.PostedDate = DateTime.Now;
                clsPost.PostedYear = DateTime.Now.Year;
                clsPost.PostedMonth = DateTime.Now.Month;
                clsPost.SlugUrl = UrlGenerator.GetUrl(clsPost.EnglishTitle);
                NewsOprations oprations = new NewsOprations();
                decimal i=oprations.NewsPost(clsPost);
                if (i > 0)
                {
                    Task.Factory.StartNew(() => new NewsOprations().UpdateData());

                    if (clsPost.IsSchedule == true)
                    {
                        try
                        {
                            SchdulePostConfig schPostConfig = new SchdulePostConfig();
                            if(schPostConfig!=null)
                            {
                                schPostConfig.PostId = i;
                                schPostConfig.PostedOn = DateTime.Now;
                                schPostConfig.ScheduleTime = DateTime.Now.AddHours(clsPost.Delay);
                                schPostConfig.TimeDelay = clsPost.Delay;
                                schulepostconfig.Insert(schPostConfig);
                                schulepostconfig.Save();
                            }
                        }
                        catch (Exception ex)
                        {

                            Log.Error(ex.ToString());
                        }
                    }
                    if(clsPost.IsSchedule == false)
                    {
                        if (Role == "SuperAdmin" || Role == "Admin")
                        {
                            if (clsPost.IsFacebookPublish == true)
                            {
                                string category = newsType.GetByID(x => x.Id == clsPost.CategoryId).NewsType;
                                var webadrs = ConfigurationManager.AppSettings["webid"];
                                Task.Factory.StartNew(() => new apiPlugin().pagePublish(clsPost.OdiaTitle, (webadrs + "/News/" + category + "/" + clsPost.PostedYear + "/" + clsPost.PostedMonth + "/" + i + "/" + clsPost.SlugUrl)));
                            }
                        }
                    }
                    return Json(new { msg = "News Posted Successfully..." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "News Posted UnSuccessful..." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json(new { msg = "News Posted UnSuccessful..." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImageDelete(string imagePath)
        {
            DeleteImage(imagePath);
            return Json(null);
        }
        [HttpGet]
        public JsonResult GetAllNews(int? page,DateTime? startdt,DateTime? enddt)
        {
            try
            {
                NewsOprations oprations = new NewsOprations();
                var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                var Role = currentUser.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                int currentuseid = int.Parse(UserId);
                if (Role == "SuperAdmin" || Role == "Admin")
                {
                    currentuseid = 0;
                }
                var item = oprations.GetAllNews(0, currentuseid, 0, page.Value, startdt, enddt);
                return Json(new { data = JsonConvert.SerializeObject(item.Tables[0]), total_pages = JsonConvert.SerializeObject(item.Tables[1]) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        public JsonResult GetNewsById(int? Id)
        {
            try
            {
                NewsPost newstyp = newsPost.GetByID(x => x.Id == Id);

                SchdulePostConfig schPostConfig = schulepostconfig.GetByID(x => x.PostId == Id);

                return Json(new { _newspost = newstyp,_npsc= schPostConfig }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        public JsonResult DeletePost(int? Id)
        {
            try
            {
                NewsPost newstyp = newsPost.GetByID(x => x.Id == Id);
                if(newstyp!=null)
                {
                    newsPost.Delete(x => x.Id == Id);
                    newsPost.Save();

                    Task.Factory.StartNew(() => new NewsOprations().UpdateData());
                    if (!string.IsNullOrEmpty(newstyp.HeaderImageName))
                    {
                        DeleteImage(newstyp.HeaderImageName);
                    }
                }
                try
                {
                    SchdulePostConfig schPostConfig = schulepostconfig.GetByID(x => x.PostId == Id);
                    if (schPostConfig != null)
                    {
                        schulepostconfig.Delete(x => x.PostId == Id);
                        schulepostconfig.Save();
                    }
                }
                catch (Exception ex)
                {

                    Log.Error(ex.ToString());
                }
                return Json(new { msg = "Post Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json(new { msg = "Post Deletion Unsucessful" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePost(ClsPost clsPost)
         {
            try
            {
                var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                var Role = currentUser.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                clsPost.IsReviewed = false;
                if (clsPost.IsSchedule == false)
                {
                    if (Role == "SuperAdmin" || Role == "Admin")
                    {
                        clsPost.ReviewedBy = int.Parse(UserId);
                        clsPost.IsReviewed = true;
                    }
                }
                clsPost.Modified = DateTime.Now;
                bool IsReviewed = false;
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    NewsPost newsPost = portalEntities.NewsPosts.Where(x=>x.Id==clsPost.Id).FirstOrDefault();
                    IsReviewed = newsPost.IsReviewed.Value;
                    newsPost.EnglishTitle = clsPost.EnglishTitle;
                    newsPost.OdiaTitle = clsPost.OdiaTitle;
                    newsPost.EngShortDesc = clsPost.EngShortDesc;
                    newsPost.ODShortDesc = clsPost.ODShortDesc;
                    newsPost.SeoMeta = clsPost.SeoMeta;
                    newsPost.Tags = clsPost.Tags;
                    newsPost.CategoryId = clsPost.CategoryId;
                    newsPost.HeaderImageName = clsPost.ImageName;
                    newsPost.ODContent = clsPost.ODContent;
                    newsPost.IsReviewed = clsPost.IsReviewed;
                    newsPost.ReviewedBy = clsPost.ReviewedBy;
                    newsPost.ModifiedOn = clsPost.Modified;
                    newsPost.Thumbnail86 = clsPost.ImageName.Replace("Img", "Thumbnail_86x64");
                    newsPost.Thumbnail210 = clsPost.ImageName.Replace("Img", "Thumbnail_210x136");
                    newsPost.Thumbnail279 = clsPost.ImageName.Replace("Img", "Thumbnail_279x220");
                    newsPost.SlugUrl = UrlGenerator.GetUrl(clsPost.EnglishTitle);
                    portalEntities.NewsPosts.Attach(newsPost);
                    portalEntities.Entry(newsPost).State = EntityState.Modified;
                    portalEntities.SaveChanges();
                    //if(IsReviewed==false)
                    {
                        if (Role == "SuperAdmin" || Role == "Admin")
                        {
                            string category = newsType.GetByID(x => x.Id == newsPost.CategoryId).NewsType;
                            var webadrs = ConfigurationManager.AppSettings["webid"];
                            Task.Factory.StartNew(() => new apiPlugin().pagePublish(newsPost.OdiaTitle, (webadrs + "/News/" + category + "/" + newsPost.PostedYear + "/" + newsPost.PostedMonth + "/" + clsPost.Id + "/" + newsPost.SlugUrl)));
                        }
                    }
                }
                Task.Factory.StartNew(() => new NewsOprations().UpdateData());
                if (clsPost.IsSchedule == true)
                {
                    try
                    {
                        SchdulePostConfig schPostConfig = schulepostconfig.GetByID(x => x.PostId == clsPost.Id);
                        if (schPostConfig != null)
                        {
                            schPostConfig.PostId = clsPost.Id;
                            schPostConfig.PostedOn = DateTime.Now;
                            schPostConfig.ScheduleTime = DateTime.Now.AddHours(clsPost.Delay);
                            schPostConfig.TimeDelay = clsPost.Delay;
                            schulepostconfig.Edit(schPostConfig);
                            schulepostconfig.Save();
                        }
                        else
                        {
                            SchdulePostConfig scPostConfig = new SchdulePostConfig();
                            scPostConfig.PostId = clsPost.Id;
                            scPostConfig.PostedOn = DateTime.Now;
                            scPostConfig.ScheduleTime = DateTime.Now.AddHours(clsPost.Delay);
                            scPostConfig.TimeDelay = clsPost.Delay;
                            schulepostconfig.Insert(scPostConfig);
                            schulepostconfig.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Update News schdule issue", ex);
                    }
                }
                else
                {
                    try
                    {
                        SchdulePostConfig schPostConfig = schulepostconfig.GetByID(x => x.PostId == clsPost.Id);
                        if (schPostConfig != null)
                        {
                            schulepostconfig.Delete(x => x.PostId == clsPost.Id);
                            schulepostconfig.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Delete News schdule issue",ex);
                    }
                }
                return Json(new { msg = "Post Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json(new { msg = "Post Updation Unsucessful" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApprovePost(int? Id)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                    NewsPost newsPost = portalEntities.NewsPosts.Where(x => x.Id == Id).FirstOrDefault();
                    newsPost.IsReviewed = true;
                    newsPost.ReviewedBy = int.Parse(UserId); ;
                    portalEntities.NewsPosts.Attach(newsPost);
                    portalEntities.Entry(newsPost).State = EntityState.Modified;
                    portalEntities.SaveChanges();
                        string category = newsType.GetByID(x => x.Id == newsPost.CategoryId).NewsType;
                        var webadrs = ConfigurationManager.AppSettings["webid"];
                        Task.Factory.StartNew(() => new apiPlugin().pagePublish(newsPost.OdiaTitle, (webadrs + "/News/" + category + "/" + newsPost.PostedYear + "/" + newsPost.PostedMonth + "/" + Id + "/" + newsPost.SlugUrl)));
                }
                Task.Factory.StartNew(() => new NewsOprations().UpdateData());

                return Json(new { msg = "Post Approved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json(new { msg = "Post Approve Unsucessful" }, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task<JsonResult> GetAnalytics(string mode="mine")
        {
            try
            {
                var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                if(mode!="mine")
                {
                    UserId = "0";
                }
                NewsOprations newsList = new NewsOprations();
                var item = await Task.Run(() => newsList.GetAnalytics(Convert.ToInt32(UserId)));
                return Json(new { TotPst = JsonConvert.SerializeObject(item.Tables[0]), TotView = JsonConvert.SerializeObject(item.Tables[1]), TdayYday = JsonConvert.SerializeObject(item.Tables[2]), Wkly = JsonConvert.SerializeObject(item.Tables[3]), Monthly= JsonConvert.SerializeObject(item.Tables[4]), DayView=JsonConvert.SerializeObject(item.Tables[5]), WkView = JsonConvert.SerializeObject(item.Tables[6]), MonthView = JsonConvert.SerializeObject(item.Tables[7]) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }

        private void DeleteImage(string imagePath)
        {
            try
            {
                string path = Server.MapPath("~/Uploads/" + imagePath);
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                    fi.Delete();
                string path1 = Server.MapPath("~/Uploads/" + imagePath.Replace("Img", "Thumbnail_210x136"));
                FileInfo fi1 = new FileInfo(path1);
                if (fi1.Exists)
                    fi1.Delete();
                string path2 = Server.MapPath("~/Uploads/" + imagePath.Replace("Img", "Thumbnail_279x220"));
                FileInfo fi2 = new FileInfo(path2);
                if (fi2.Exists)
                    fi2.Delete();
                string path3 = Server.MapPath("~/Uploads/" + imagePath.Replace("Img", "Thumbnail_86x64"));
                FileInfo fi3 = new FileInfo(path3);
                if (fi3.Exists)
                    fi3.Delete();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string ImageUrl = string.Empty;
            string filename = string.Empty;
            NewsPost nps = new NewsPost();
            try
            {
                int NewsId = 0;
                string FolderName = "";
                string path = "";
                if (Request.Params["NewsId"] =="0")
                {
                    using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                    {
                        NewsId = portalEntities.NewsPosts.Where(x => x.PostedDate.Value.Year == DateTime.Now.Year && x.PostedDate.Value.Month == DateTime.Now.Month && x.PostedDate.Value.Day == DateTime.Now.Day).ToList().Count;
                    }
                    NewsId = NewsId + 1;

                    FolderName = DateTime.Now.Date.ToString("ddMMyyyy");
                    path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId + "/Content");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                    {
                        decimal newsID = Convert.ToDecimal(Request.Params["NewsId"]);
                        nps = portalEntities.NewsPosts.Where(x => x.Id == newsID).FirstOrDefault();
                        string ImageName = "";
                        if (nps != null)
                        {
                            ImageName = nps.HeaderImageName;
                            FolderName = ImageName.Split('/')[0];
                            NewsId = int.Parse(ImageName.Split('/')[1]);
                        }
                    }
                    path = Server.MapPath("~/Uploads/" + FolderName + "/" + NewsId + "/Content");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                string imgId = DateTime.Now.Minute + "" + DateTime.Now.Second;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var postedFile = System.Web.HttpContext.Current.Request.Files["Image"];
                    if (postedFile.ContentLength > 0)
                    {
                        ImageUrl = Path.Combine(path, "Img_" + imgId + ".jpg");
                        Stream strm = postedFile.InputStream;
                        int flstatus = GenerateThumbnails(0.2, strm, ImageUrl, 400, 210);
                        filename = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/Uploads/" + FolderName + "/" + NewsId + "/Content/Img_" + imgId + ".jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return Json(Convert.ToString(filename), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteContent(string ImgUrl)
        {
            try
            {
                ImgUrl = ImgUrl.Replace(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority,"");
                string path = Server.MapPath("~"+ ImgUrl);
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                    fi.Delete();
                return Json("Image Deleted Successfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}