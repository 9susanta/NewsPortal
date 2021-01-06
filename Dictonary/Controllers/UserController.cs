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
    public class UserController : Controller
    {
        private IOperation<tblUses> useroper = null;
        public UserController()
        {
            this.useroper = new OpertionClass<tblUses>();
        }
        public UserController(IOperation<tblUses> _useroper)
        {
            this.useroper = _useroper;
        }
        // GET: User
        public ActionResult Index()
        {
            var model = useroper.GetAll();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Users _user)
        {
            tblUses uses = new tblUses();
            uses.FullName = _user.FullName;
            uses.UserName = _user.UserName;
            uses.EmailId = _user.EmailID;
            uses.Password = _user.Password;
            uses.PhoneNo = _user.PhoneNo;
            uses.IsActive = true;
            uses.IsDelete = true;
            useroper.Insert(uses);
            useroper.Save();
            return View();
        }
        public ActionResult EditUser(Users _user)
        {
            tblUses uses = useroper.GetByID(_user.UserId);
            uses.Id = _user.UserId;
            uses.FullName = _user.FullName;
            uses.UserName = _user.UserName;
            uses.EmailId = _user.EmailID;
            uses.Password = _user.Password;
            uses.PhoneNo = _user.PhoneNo;
            uses.IsActive = true;
            uses.IsDelete = true;
            useroper.Edit(uses);
            useroper.Save();
            return View();
        }
        public ActionResult DeleteUser(Users _user)
        {
            useroper.Delete(_user.UserId);
            useroper.Save(); 
            return View();
        }
        public ActionResult LoginUser()
        {
            return View();
        }
        public ActionResult LoginUser(Users _user)
        {
            using (NewsPortalEntities dBEntities = new NewsPortalEntities())
            {
                var userexist = dBEntities.tblUses.Where(x => x.UserName == _user.UserName && x.Password == _user.Password).ToList();
                if(userexist.Count==0)
                {
                    return View("Home", "About");
                   
                }
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Sid,userexist.FirstOrDefault().UserName),
                    new Claim(ClaimTypes.Email,userexist.FirstOrDefault().EmailId),
                    new Claim(ClaimTypes.MobilePhone,userexist.FirstOrDefault().PhoneNo),
                },"ApplicationCookie");
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
            }
            return View();
        }
        public ActionResult LogoutUser()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            Session.Abandon();
            return View();
        }
        public ActionResult ChangePassword(Users _user)
        {
            return View();
        }
    }
}