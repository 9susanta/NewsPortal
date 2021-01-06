using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ValLangController : Controller
    {
        private IOperation<tblLanguage> useroper = null;
        public ValLangController()
        {
            this.useroper = new OpertionClass<tblLanguage>();
        }
        public ValLangController(IOperation<tblLanguage> _useroper)
        {
            this.useroper = _useroper;
        }
        // GET: ValLang
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLang()
        {
            var model = useroper.GetAll();
            return Json(model);
        }
        [HttpPost]
        public JsonResult InsertLang(clsLanguage _lang)
        {
            tblLanguage lang = new tblLanguage();
            lang.Language = _lang.LangName;
            lang.IsActive = true;
            lang.IsDeleted = true;
            useroper.Insert(lang);
            useroper.Save();

            return Json(lang);
        }
        [HttpPost]
        public JsonResult UpdateLang(clsLanguage _lang)
        {
            tblLanguage lang = useroper.GetByID(_lang.Id);
            lang.Language = _lang.LangName;
            useroper.Edit(lang);
            useroper.Save();

            return Json(lang);
        }
        [HttpPost]
        public JsonResult DeleteLang(int Id)
        {
            useroper.Delete(Id);
            useroper.Save();
            return Json(Id);
        }
    }
}