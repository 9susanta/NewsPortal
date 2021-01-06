using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class NewsTypeController : BaseController
    {
        private IOperation<tblNewsType> newsType = null;
        public NewsTypeController()
        {
            this.newsType = new OpertionClass<tblNewsType>();
        }
        public NewsTypeController(IOperation<tblNewsType> _newsType)
        {
            this.newsType = _newsType;
        }
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetNewsType(int? page, int? pageSize)
        {
            try
            {
                ClsPaged<tblNewsType> objNewsType = new ClsPaged<tblNewsType>();
                var model = newsType.GetAll(i => i.IsDeleted == false).ToList();
                return Json(objNewsType.Get(page, pageSize, model), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpGet]
        public JsonResult GetNewsTypes()
        {
            try
            {
                var model = newsType.GetAll(i => i.IsDeleted == false).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpPost]
        public JsonResult InsertNewsType(NewsType _newsTyp)
        {
            try
            {
                clsNewsType clsNewsType = new clsNewsType();
                var itemfound = clsNewsType.CheckNewsType(_newsTyp.NewsTypeName);
                if (itemfound > 0)
                {
                    return Json(new { msg = "This Record is already Exist" }, JsonRequestBehavior.AllowGet);
                }
                tblNewsType newstyp = new tblNewsType();
                newstyp.NewsType = _newsTyp.NewsTypeName;
                newstyp.OdiaName = _newsTyp.NewsTypeOdia;
                newstyp.IsMenu = _newsTyp.IsMenu;
                newstyp.IsDeleted = false;
                newsType.Insert(newstyp);
                newsType.Save();
                return Json(newstyp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpPost]
        public JsonResult UpdateNewsType(NewsType _newsTyp)
        {
            try
            {
                clsNewsType clsNewsType = new clsNewsType();
                var itemfound = clsNewsType.CheckNewsType(_newsTyp.NewsTypeName);
                if (itemfound > 0)
                {
                    tblNewsType newstype = newsType.GetByID(x => x.Id == _newsTyp.NewsTypeId);
                    newstype.OdiaName = _newsTyp.NewsTypeOdia;
                    newstype.IsMenu = _newsTyp.IsMenu;
                    newsType.Edit(newstype);
                    newsType.Save();
                    return Json(new { msg = "This Record is Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblNewsType newstyp = newsType.GetByID(x => x.Id == _newsTyp.NewsTypeId);
                    newstyp.NewsType = _newsTyp.NewsTypeName;
                    newstyp.OdiaName = _newsTyp.NewsTypeOdia;
                    newstyp.IsMenu = _newsTyp.IsMenu;
                    newsType.Edit(newstyp);
                    newsType.Save();
                    return Json(newstyp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;

        }
        [HttpPost]
        public JsonResult DeleteNewsType(int Id)
        {
            try
            {
                tblNewsType newstyp = newsType.GetByID(x => x.Id == Id);
                newstyp.IsDeleted = true;
                newsType.Edit(newstyp);
                newsType.Save();
                return Json(Id);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        public ActionResult menuLayout()
        {
            try
            {
                var menuItem = System.Web.HttpRuntime.Cache["menuItem"] as List<tblNewsType>;
                if (menuItem != null)
                {
                    return PartialView("_MenuLayout", menuItem);
                }
                else
                {
                    var model = newsType.GetAll(i => i.IsDeleted == false && i.IsMenu == true).ToList();
                    System.Web.HttpRuntime.Cache.Insert("menuItem", model, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                    return PartialView("_MenuLayout", model);
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