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
    public class RightsController : BaseController
    {
        private IOperation<tblRight> rights = null;
        public RightsController()
        {
            this.rights = new OpertionClass<tblRight>();
        }
        public RightsController(IOperation<tblRight> _rights)
        {
            this.rights = _rights;
        }
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetRight(int? page, int? pageSize)
        {
            try
            {
                ClsPaged<tblRight> listRights = new ClsPaged<tblRight>();
                var model = rights.GetAll(i => i.IsDeleted == false).ToList();
                return Json(listRights.Get(page, pageSize, model), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpGet]
        public JsonResult GetRights()
        {
            try
            {
                var model = rights.GetAll(i => i.IsDeleted == false).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult InsertRight(clsRights _rights)
        {
            try
            {
                tblRight rigt = new tblRight();
                rigt.RightsName = _rights.RightsName;
                rigt.IsDeleted = false;
                rights.Insert(rigt);
                rights.Save();
                return Json(rigt);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult UpdateRight(clsRights _rights)
        {
            try
            {
                tblRight rigt = rights.GetByID(x => x.RightsId == _rights.RightsId);
                rigt.RightsName = _rights.RightsName;
                rights.Edit(rigt);
                rights.Save();
                return Json(rigt);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult DeleteRight(int Id)
        {
            try
            {
                tblRight rigt = rights.GetByID(x => x.RightsId == Id);
                rigt.IsDeleted = true;
                rights.Edit(rigt);
                rights.Save();
                return Json(Id);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
    }
}
