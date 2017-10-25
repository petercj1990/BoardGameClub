using System.Web;
using System.Web.Optimization;

namespace BoardGameClub
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/mini-spa/style").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/ui-bootstrap-csp.css",
                      "~/Profile/styles/main.css"));
        
            bundles.Add(new ScriptBundle("~/bundles/mini-spa/script")
                      .Include("~/Scripts/angular.js")
                      .Include("~/Scripts/angular-route.js")
                      .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                      .Include("~/MainPage/BGCApp.js")
                      .Include("~/MainPage/scripts/services/DataService.js")
                      .Include("~/MainPage/scripts/services/UserService.js")
                      .Include("~/MainPage/scripts/controllers/about.js")
                      .Include("~/MainPage/scripts/controllers/main.js")
                      .Include("~/Profile/PlayerApp.js")
                      .Include("~/Profile/scripts/controllers/about.js")
                      .Include("~/Profile/scripts/controllers/Player.js")
                      .Include("~/Profile/scripts/controllers/boardgameModal.js")
                      .Include("~/Profile/scripts/services/DataService.js")
                      .Include("~/Profile/scripts/services/UserService.js")
                );
        }
    }
}
