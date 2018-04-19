using System.Web;
using System.Web.Optimization;

namespace BoardGameClub
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new Bundle("~/bundles/jqueryui").Include(
              "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new Bundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

         
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new Bundle("~/bundles/modernizr").Include(
                              "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/jquery.ui.core.css",
                      "~/Content/themes/base/jquery.ui.resizable.css",
                      "~/Content/themes/base/jquery.ui.selectable.css",
                      "~/Content/themes/base/jquery.ui.accordion.css",
                      "~/Content/themes/base/jquery.ui.autocomplete.css",
                      "~/Content/themes/base/jquery.ui.button.css",
                      "~/Content/themes/base/jquery.ui.dialog.css",
                      "~/Content/themes/base/jquery.ui.slider.css",
                      "~/Content/themes/base/jquery.ui.tabs.css",
                      "~/Content/themes/base/jquery.ui.datepicker.css",
                      "~/Content/themes/base/jquery.ui.progressbar.css",
                      "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new Bundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/jquery.ui.core.css",
                      "~/Content/themes/base/jquery.ui.autocomplete.css",
                      "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/mini-spa/style").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/ui-bootstrap-csp.css",
                      "~/Profile/styles/main.css",
                      "~/Scripts/ngImgCrop-master/ngImgCrop-master/compile/unminified/ng-img-crop.css"
                      ));
        
            bundles.Add(new Bundle("~/bundles/mini-spa/script")
                      .Include("~/Scripts/angular.js")
                      .Include("~/Scripts/angular-animate.js")
                      .Include("~/Scripts/angular-sanitize.js")
                      .Include("~/Scripts/angular-route.js")
                      .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                      .Include("~/Scripts/ng-file-upload.js")
                      .Include("~/Scripts/ng-file-upload-shim.js")
                      .Include("~/Scripts/angular-drag-and-drop-lists.js")
                      .Include("~/Scripts/ngImgCrop-master/ngImgCrop-master/compile/unminified/ng-img-crop.js")
                      .Include("~/Scripts/ngImgCrop-master/ngImgCrop-master/source/js/ng-img-crop.js")
                      .Include("~/MainPage/BGCApp.js")
                      .Include("~/MainPage/scripts/services/DataService.js")
                      .Include("~/MainPage/scripts/services/UserService.js")
                      .Include("~/MainPage/scripts/controllers/about.js")
                      .Include("~/MainPage/scripts/controllers/main.js")
                      .Include("~/Profile/PlayerApp.js")
                      .Include("~/Profile/scripts/controllers/about.js")
                      .Include("~/Profile/scripts/controllers/Player.js")
                      .Include("~/Profile/scripts/controllers/boardgameModal.js")
                      .Include("~/Profile/scripts/controllers/BoardGame.js")
                      .Include("~/Profile/scripts/services/DataService.js")
                      .Include("~/Profile/scripts/services/UserService.js")
                      .Include("~/Profile/scripts/directives/searchBar.js")
                      .Include("~/PlaySessions/PlaySessionApp.js")
                      .Include("~/PlaySessions/scripts/services/DataService.js")
                      .Include("~/PlaySessions/scripts/controllers/playSession.js")
                );
        }
    }
}
