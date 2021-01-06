using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(NewsPortal.App_Start.Startup))]

namespace NewsPortal.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                LoginPath = new PathString("/Users/Login"),
                LogoutPath=new PathString("/Users/Logout"),
                CookieHttpOnly = true,
                CookieName ="NewsPortalUser",
                ReturnUrlParameter = "ReturnUrl",
                CookieSecure = CookieSecureOption.SameAsRequest, //Use CookieSecureOption.Always if you intend to serve cookie in SSL/TLS (Https)
                SlidingExpiration = true,
            });
            
        }
    }
}
