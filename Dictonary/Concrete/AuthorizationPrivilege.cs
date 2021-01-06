using NewsPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Concrete
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizationPrivilegeAttribute : AuthorizeAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var principal = filterContext.RequestContext.HttpContext.User as ClaimsPrincipal;
            #region comment
            //if (!principal.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new RedirectResult("~/auth/signin");
            //    return;
            //}
            #endregion
            if (!string.IsNullOrEmpty(ClaimValue))
            {
                var claimValue = ClaimValue.Split(',');
                if (!(principal.HasClaim(x => x.Type == ClaimType && claimValue.Any(v => v == x.Value) && x.Issuer == Constants.Issuer)))
                {
                    filterContext.Result = new RedirectResult("~/Views/Error/AccessDenied.html");
                }
            }
            else
            {
                if (!principal.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("~/Views/Error/AccessDenied.html");
                }
            }
            base.OnAuthorization(filterContext);
        }
       
    }
}
//public override void OnActionExecuting(ActionExecutingContext filterContext)
//{
//    try
//    {
//        bool skipAuthorizationPrivilege = filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAuthorizationPrivilege), true) ||
//                                           filterContext.ActionDescriptor.IsDefined(typeof(SkipAuthorizationPrivilege), true);
//        var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
//        var actionName = filterContext.ActionDescriptor.ActionName;
//        var user = filterContext.HttpContext.User;
//        if (!skipAuthorizationPrivilege)
//        {
//            var currentUser = (ClaimsPrincipal)Thread.CurrentPrincipal;
//            //System.Threading.Thread.CurrentPrincipal = currentUser;
//            //HttpContext.Current.User = currentUser;

//            var userName = currentUser.Claims.Where(x=>x.Type==ClaimTypes.Sid).Select(c=>c.Value).SingleOrDefault();
//            if (string.IsNullOrEmpty(userName))
//            {
//                filterContext.Result = new ViewResult()
//                {
//                    ViewName="~/Views/Error/AccessDenied.cshtml"
//                };
//            }
//        }
//    }
//    catch (Exception ex)
//    {

//        throw;
//    }
//    base.OnActionExecuting(filterContext);
//}