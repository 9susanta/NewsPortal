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
    public class UserManagementController : BaseController
    {
        private IOperation<tblRole> userrole = null;
        public UserManagementController()
        {
            this.userrole = new OpertionClass<tblRole>();
        }
        public UserManagementController(IOperation<tblRole> _userrole)
        {
            this.userrole = _userrole;
        }
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetRole(int? page, int? pageSize)
        {
            ClsPaged<tblRole> listRole = new ClsPaged<tblRole>();
            var model = userrole.GetAll(i => i.IsDeleted == false).ToList();
            return Json(listRole.Get(page, pageSize, model),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertRole(clsRole _role)
        {
            tblRole lang = new tblRole();
            lang.RoleName = _role.RoleName;
            lang.IsDeleted = false;
            userrole.Insert(lang);
            userrole.Save();
            return Json(lang);
        }
        [HttpPost]
        public JsonResult UpdateRole(clsRole _role)
        {
            tblRole lang = userrole.GetByID(x => x.RoleId == _role.RoleId);
            lang.RoleName = _role.RoleName;
            userrole.Edit(lang);
            userrole.Save();
            return Json(lang);
        }
        [HttpPost]
        public JsonResult DeleteRole(int? Id)
        {
            tblRole lang = userrole.GetByID(x => x.RoleId == Id);
            lang.IsDeleted = true;
            userrole.Save();
            return Json(Id);
        }
    }
}