using log4net;
using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class HomeController: BaseController
    {
        private IOperation<NewsPost> newsOperation = null;
        private IOperation<tblContact> tblContact = null;
        ILog Log = log4net.LogManager.GetLogger(typeof(NewsOprationsController));
        public HomeController()
        {
            this.newsOperation = new OpertionClass<NewsPost>();
            this.tblContact = new OpertionClass<tblContact>();
        }
        public HomeController(IOperation<NewsPost> _newsOperation)
        {
            this.newsOperation = _newsOperation;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home - Khabar Odia";
            var request = HttpContext.Request.Url;
            ViewBag.Url = request;
            ViewBag.ImgUrl = HttpContext.Request.Url.Scheme+"://"+ HttpContext.Request.Url.Authority + "/Images/logo/default.png";
            ViewBag.Desc = "Khabar Odia is one of the leading web platform which brings up latest,crime,politics,entertainment,sports and many more news from round the globle to its readers in Odia. Odia is one of the oldest indian language which use brodly use by Indian state odisha. Also Khabar Odia is commited to reach more and more odia people with its unbaised news in their langauge only .";
            return View();
        }
        public ActionResult Category(int? categoryId,string category)
        {
            ViewBag.Title = category+" - Khabar Odia";
            var request = HttpContext.Request.Url;
            ViewBag.Url = request;
            ViewBag.ImgUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/Images/logo/default.png";
            ViewBag.Desc = "Khabar Odia is one of the leading web platform which brings up latest,crime,politics,entertainment,sports and many more news from round the globle to its readers in Odia. Odia is one of the oldest indian language which use brodly use by Indian state odisha. Also Khabar Odia is commited to reach more and more odia people with its unbaised news in their langauge only .";
            return View();
        }
        public async Task<JsonResult> GetFirstTimeCategoryData(int? categoryId)
        {
            try
            {
                NewsOprations np = new NewsOprations();
                DataSet ds = await Task.Run(() => np.GetClientCategory(categoryId.Value));
                var result = new { latest = JsonConvert.SerializeObject(ds.Tables[0],Formatting.None), popular = JsonConvert.SerializeObject(ds.Tables[1],Formatting.None), category = JsonConvert.SerializeObject(ds.Tables[2], Formatting.None),paging= JsonConvert.SerializeObject(ds.Tables[3], Formatting.None) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        public async Task<JsonResult> GetResultSection(int? categoryId, int? pageIndex,string section)
        {
            try
            {
                NewsOprations np = new NewsOprations();
                DataTable dataTable = await Task.Run(() => np.GetSectionData(section, pageIndex.Value, categoryId.Value));
                var result = new { data = JsonConvert.SerializeObject(dataTable, Formatting.None) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;

        }
        public async Task<JsonResult> GetCategoryData(int? categoryId, int? page, int? pageSize)
        {
            NewsOprations np = new NewsOprations();
            DataSet ds= await Task.Run(() => np.GetNews(page.Value, pageSize.Value, categoryId.Value));
            return Json(new { data = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None),totalcount= JsonConvert.SerializeObject(ds.Tables[1], Formatting.None) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryDataByName(string category, int? page, int? pageSize)
        {
            //NewsOprations np = new NewsOprations();
            //NewsList nl = np.GetNewsBySearch(page.Value, category);
            //nl.CurrentPages = page.Value;
            //nl.TotalPages = (int)Math.Ceiling((decimal)nl.totalCount / pageSize.Value);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Topic(string search)
        {
            return View();
        }
        public JsonResult GetTopicData(int? Id)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var item = (from news in portalEntities.NewsPosts
                                where news.Id == Id
                                select new { news.OdiaTitle, news.Tags, news.SeoMeta, news.PostedOn, news.ODShortDesc, news.EnglishTitle, news.ODContent }).FirstOrDefault();
                    return Json(item, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;

        }
        public async Task<ActionResult> News(string Category, int? Year, int? Month, decimal? Id,string SlugUrl)
        {
            try
            {
                NewsOprations np = new NewsOprations();
                DataSet ds = await Task.Run(() => np.GetNewPost(Id.Value, 1));
                if (ds == null)
                {
                    return View();
                }
                ViewBag.Url = HttpContext.Request;
                return View(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return View();
        }
        public async Task<JsonResult> GetNewsData(decimal Id)
        {
            try
            {
                NewsOprations np = new NewsOprations();
                DataSet ds = await Task.Run(() => np.GetNewPost(Id,2));
                if (ds==null)
                {
                    return null;
                }
                var result = new { latest = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None), popular = JsonConvert.SerializeObject(ds.Tables[1], Formatting.None),related = JsonConvert.SerializeObject(ds.Tables[2], Formatting.None), Prev = JsonConvert.SerializeObject(ds.Tables[3], Formatting.None), Next = JsonConvert.SerializeObject(ds.Tables[4], Formatting.None) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public async Task<JsonResult> GetHomePage()
        {
            try
            {
                NewsOprations np = new NewsOprations();
                DataSet ds = await Task.Run(() => np.GetHomePage());
                if (ds == null)
                {
                    return null;
                }
                var result = new { latest = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None), datacat = JsonConvert.SerializeObject(ds.Tables[1], Formatting.None), svmenu = JsonConvert.SerializeObject(ds.Tables[2], Formatting.None), plrnw = JsonConvert.SerializeObject(ds.Tables[3], Formatting.None), phtnw = JsonConvert.SerializeObject(ds.Tables[4], Formatting.None) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        public async Task<JsonResult> GetTopic(string topic, int? pageIndex = 1)
        {
            try
            {
                NewsOprations newsList = new NewsOprations();
                var item = await Task.Run(()=> newsList.GetNewsBySearch(pageIndex.Value, topic));
                return Json(new { latestdata = JsonConvert.SerializeObject(item.Tables[0]), populardata = JsonConvert.SerializeObject(item.Tables[1]),data= JsonConvert.SerializeObject(item.Tables[2]),count= JsonConvert.SerializeObject(item.Tables[3]) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        public JsonResult SendEmail(tblContact tbl)
        {
            try
            {
                tbl.PostedOn = DateTime.Now;
                tblContact.Insert(tbl);
                tblContact.Save();
                return Json(new{latestdata = "Success"});
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
    }
}