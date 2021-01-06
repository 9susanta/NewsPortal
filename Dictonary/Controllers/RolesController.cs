using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class RolesController : BaseController
    {
        private IOperation<tblRole> roles = null;
        public RolesController()
        {
            this.roles = new OpertionClass<tblRole>();
        }
        public RolesController(IOperation<tblRole> _roles)
        {
            this.roles = _roles;
        }
        [AuthorizationPrivilege(ClaimType = ClaimTypes.Role, ClaimValue = "Admin,SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetRole(int? page, int? pageSize)
        {
            try
            {
                ClsPaged<tblRole> listRoles = new ClsPaged<tblRole>();
                var model = roles.GetAll(i => i.IsDeleted == false).ToList();
                return Json(listRoles.Get(page, pageSize, model), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpGet]
        public JsonResult GetRoles()
        {
            try
            {
                var model = roles.GetAll(i => i.IsDeleted == false).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpPost]
        public JsonResult InsertRole(clsRole _roles)
        {
            try
            {
                ClsRole clsRole = new ClsRole();
                var itemfound = clsRole.CheckRoleName(_roles.RoleName);
                if (itemfound > 0)
                {
                    return Json(new { msg = "This Record is already Exist" }, JsonRequestBehavior.AllowGet);
                }
                tblRole rigt = new tblRole();
                rigt.RoleName = _roles.RoleName;
                rigt.IsDeleted = false;
                roles.Insert(rigt);
                roles.Save();
                return Json(rigt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;

        }
        [HttpPost]
        public JsonResult UpdateRole(clsRole _roles)
        {
            try
            {
                ClsRole clsRole = new ClsRole();
                var itemfound = clsRole.CheckRoleName(_roles.RoleName);
                if (itemfound > 0)
                {
                    return Json(new { msg = "This Record is already Exist" }, JsonRequestBehavior.AllowGet);
                }
                tblRole rigt = roles.GetByID(x => x.RoleId == _roles.RoleId);
                rigt.RoleName = _roles.RoleName;
                roles.Edit(rigt);
                roles.Save();
                return Json(rigt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
        [HttpPost]
        public JsonResult DeleteRole(int Id)
        {
            try
            {
                tblRole rigt = roles.GetByID(x => x.RoleId == Id);
                rigt.IsDeleted = true;
                roles.Edit(rigt);
                roles.Save();
                return Json(Id);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return null;
        }
    }
}