using System.Web;
using System.Web.Optimization;

namespace NewsPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862

        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations=true;

            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular", "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.7.8/angular.min.js").Include(
                        "~/Scripts/angular.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/route", "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.7.8/angular-route.min.js").Include(
                "~/Scripts/angular-route.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/resources", "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.7.8/angular-resource.min.js").Include(
                 "~/Scripts/angular-resource.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sanitize", "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.7.8/angular-sanitize.min.js").Include(
                 "~/Scripts/angular-sanitize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr", "https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/js/bootstrap.min.js").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/propper", "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.15.0/umd/popper.min.js").Include(
                      "~/Scripts/umd/popper.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                      "~/Scripts/Calender/calendar.js"));


            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                      "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                      "~/Scripts/Home/Home.js"));

            bundles.Add(new ScriptBundle("~/bundles/cat").Include(
                      "~/Scripts/Category/Category.js"));

            bundles.Add(new ScriptBundle("~/bundles/news").Include(
                     "~/Scripts/News/News.js"));

            bundles.Add(new ScriptBundle("~/bundles/PrivacyPolicy").Include(
                     "~/Scripts/PrivacyPolicy/PrivacyPolicy.js"));

            bundles.Add(new ScriptBundle("~/bundles/top").Include(
                    "~/Scripts/Topic/Topic.js"));

            bundles.Add(new ScriptBundle("~/bundles/lazy").Include(
                     "~/Scripts/LazyLoading/angular-bn-lazy-src.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapcss", "https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/css/bootstrap.min.css").Include(
                "~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/admin-style.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/stylelogin.css"));

            bundles.Add(new StyleBundle("~/Content/calender").Include("~/Scripts/Calender/calender.css"));

            bundles.Add(new StyleBundle("~/Content/IconCss", "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.11.2/css/all.min.css").Include(
                     "~/Content/fontawesome/css/all.min.css"));
        }
    }
}
