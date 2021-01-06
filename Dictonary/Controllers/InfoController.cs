using NewsPortal.Concrete;
using NewsPortal.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        private IOperation<tblContact> tblcon = null;
        public InfoController()
        {
            this.tblcon = new OpertionClass<tblContact>();
        }
        public InfoController(IOperation<tblContact> _tblcon)
        {
            this.tblcon = _tblcon;
        }
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Details()
        {
            try
            {
                var result = new { Info = JsonConvert.SerializeObject(tblcon.GetAll(x => x.Id > 0).OrderByDescending(x => x.PostedOn), Formatting.None) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpPost]
        public JsonResult Delete(decimal Id)
        {
            try
            {
                tblcon.Delete(x => x.Id == Id);
                tblcon.Save();
                var result = new { Info = "Success", Formatting.None};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpDelete]
        public JsonResult SeletedDelete(clsInfo _clsInfo)
        {
            try
            {
                var item = tblcon.GetAll(x => _clsInfo.Ids.Contains(x.Id)).ToList();
                tblcon.BulkDelete(item);
                tblcon.Save();
                var result = new { Info = "Success", Formatting.None };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
    }
}