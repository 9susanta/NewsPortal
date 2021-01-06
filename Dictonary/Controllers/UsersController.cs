using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NewsPortal.Common;
using NewsPortal.Concrete;
using NewsPortal.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class UsersController : BaseController
    {
        private IOperation<tblUser> users = null;
        public HttpCookie coockieuser = new HttpCookie("User");
        ILog Log = log4net.LogManager.GetLogger(typeof(UsersController));
        public UsersController()
        {
            this.users = new OpertionClass<tblUser>();
        }
        public UsersController(IOperation<tblUser> _users)
        {
            this.users = _users;
        }
        [AuthorizationPrivilege]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            if (UserSessionModel != null)
            {
                return RedirectToAction("Logout", "Users");
            }
            string username = "";
            string password = "";
            if(Request.Cookies["User"]!=null)
            {
                username = Request.Cookies["User"]["UserName"];
                password = Request.Cookies["User"]["PassWord"];
            }
            ViewBag.UserName = username;
            ViewBag.ReturnUrl = ReturnUrl;
            clsUser _users = new clsUser();
            return View(_users);
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(clsUser _user, string returnUrl = default(string))
        {
            string rtnUrl=ViewBag.ReturnUrl;
            string rememberme = "";
            TempData["loginFailedMessage"] = "";
            if(ModelState.IsValid)
            {
                if(rememberme=="true")
                {
                    coockieuser.Values["UserName"] = _user.UserName;
                    coockieuser.Values["PassWord"] = new Helper().Encrypt(_user.Password);
                    coockieuser.Expires = DateTime.Now.AddHours(7);
                    coockieuser.HttpOnly = true;
                    Response.Cookies.Add(coockieuser);
                }
                else
                {
                    if(Request.Cookies["User"]!=null)
                    {
                        Response.Cookies["User"].Expires = DateTime.Now.AddHours(-1);
                    }
                }
                var loginResult = new Usermanament().Login(_user.UserName, new Helper().Encrypt(_user.Password));
                if(loginResult!=null)
                {
                    var userSession = new UserSessionModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = loginResult.FullName
                    };

                    var identity = new ClaimsIdentity(AuthenticationHelper.CreateClaim(userSession, loginResult.UserName, loginResult.UserId,loginResult.RoleName.ToString()),DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(1)
                    },identity);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Dashbord", "NewsOprations");
                }
                else
                {
                    TempData["loginFailedMessage"] = "Entered UserName and Password is Wrong";
                }
            }
            else
            {
                if(string.IsNullOrEmpty(_user.UserName))
                {
                    TempData["loginFailedMessage"] = "Please Enter Your Username";
                }
                else if(string.IsNullOrEmpty(_user.Password))
                {
                    TempData["loginFailedMessage"] = "Please Enter Your Password";
                }
            }
            return View();
        }
        [AllowAnonymous]
        [SkipAuthorizationPrivilege]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult UserProfile()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetCurrentUser()
        {
            var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
            int UserID = int.Parse(UserId);
            var _user = new NewsOprations().getCurrentUserId(UserID);
            return Json(new { user = JsonConvert.SerializeObject(_user) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetCurrentUserInfo(clsUser _user)
        {
            var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
            int UserID = int.Parse(UserId);
            tblUser user = users.GetByID(x => x.UserId == UserID);
            user.FullName = _user.FullName;
            user.Email = _user.Email;
            user.Phone = _user.Phone;
            user.DateUpdate = DateTime.Now;
            users.Edit(user);
            users.Save();
            return Json(new { user = "Sucess" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(UpdateAccount _user)
        {
            string message = "";
            bool status = false;

            if (ModelState.IsValid)
            {
                var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = currentUser.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                int UserID = int.Parse(UserId);
                _user.CurrentPassword = new Helper().Encrypt(_user.CurrentPassword);
                tblUser user = users.GetByID(x => x.UserId == UserID && x.Password== _user.CurrentPassword);
                if(user!=null)
                {
                    user.Password = new Helper().Encrypt(_user.ConfirmPassword);
                    users.Edit(user);
                    users.Save();
                    return RedirectToAction("Logout");
                }
                else
                {
                    TempData["valid"]= "You Enter Wrong Current Password";
                }
            }
           return View();
        }
        public ActionResult ChangePasswordByUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePasswordByUser(clsUser _user)
        {
            return View();
        }
        [HttpGet]
        [AuthorizationPrivilege]
        public JsonResult GetUser(int? page, int? pageSize)
        {
            ClsPaged<tblUser> listUser = new ClsPaged<tblUser>();
            var model = users.GetAll(i=>i.IsDeleted==false).ToList();
            return Json(listUser.Get(page, pageSize, model), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUsers()
        {
            try
            {
                var model = users.GetAll(i => i.IsDeleted == false).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        [AuthorizationPrivilege]
        public JsonResult InsertUser(clsUser _user)
        {
            try
            {
                var usernm = users.GetByID(x => x.UserName == _user.UserName);
                if (usernm != null)
                {
                    return Json(new { usernm = "UserName already Exist" });
                }
                var emailId = users.GetByID(x => x.Email == _user.Email);
                if (emailId != null)
                {
                    return Json(new { emailId = "Email already Exist" });
                }
                var mobileNo = users.GetByID(x => x.Phone == _user.Phone);
                if (mobileNo != null)
                {
                    return Json(new { mobileNo = "Mobile No. already Exist" });
                }
                tblUser user = new tblUser();
                user.UserName = _user.UserName;
                user.FullName = _user.FullName;
                user.Email = _user.Email;
                user.Password = new Helper().Encrypt("welcome_123");
                user.Phone = _user.Phone;
                user.RoleId = _user.RoleId;
                user.IsBlocked = false;
                user.DateCreate = DateTime.Now;
                user.IsDeleted = false;
                users.Insert(user);
                users.Save();
                return Json(user);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult UpdateUser(clsUser _user)
        {
            try
            {
                    tblUser user = users.GetByID(x => x.UserId == _user.UserId);
                    user.FullName = _user.FullName;
                    user.Email = _user.Email;
                    user.Phone = _user.Phone;
                    user.DateUpdate = DateTime.Now;
                    users.Edit(user);
                    users.Save();
                    return Json(new { usernm = "User Information Updated  Successfully" });
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult DeleteUser(int Id)
        {
            try
            {
                tblUser user = users.GetByID(x => x.UserId == Id);
                user.IsDeleted = true;
                users.Edit(user);
                users.Save();
                return Json(new { msg = "User Deleted Successfully" });
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult UserReset(int Id)
        {
            try
            {
                tblUser user = users.GetByID(x => x.UserId == Id);
                user.Password = new Helper().Encrypt("welcome_123");
                users.Edit(user);
                users.Save();
                return Json(new { msg = "User Reset Successfully" });
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
        [HttpPost]
        public JsonResult UserBlock(int Id,bool status)
        {
            try
            {
                tblUser user = users.GetByID(x => x.UserId == Id);
                user.IsBlocked = !status;
                users.Edit(user);
                users.Save();
                return Json(new { msg = status });
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
            return Json(null);
        }
    }
}