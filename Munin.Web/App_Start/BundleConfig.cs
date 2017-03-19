using System.Web;
using System.Web.Optimization;

namespace Munin.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-ui/select.js",
                        "~/Scripts/angular-ui/select2.js",
                        "~/Scripts/angular-filter.js",
                        "~/Scripts/angular-sanitize.min.js",
                        "~/Scripts/momment.js",
                        "~/Scripts/select.js",
                        "~/Scripts/i18n/angular-locale_da-dk.js",
                        "~/Scripts/angular-ui/ui-bootstrap.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                        "~/Scripts/datetimepicker.js",
                        "~/Scripts/datetimepicker.templates.js",
                        "~/Scripts/App/MuninApp.js",
                        "~/Scripts/App/Filters.js",
                        "~/Scripts/App/PictureListCtrl.js",
                        "~/Scripts/App/PictureFormCtrl.js",
                        "~/Scripts/App/ClipsFormCtrl.js",
                        "~/Scripts/App/ClipsListCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(                      
                    "~/Content/bootstrap.css",
                    "~/Content/font-awesome.css",
                    "~/Content/datetimepicker.css",
                    "~/Content/select2.css",
                    "~/Content/select2-bootstrap.css",
                    "~/Content/select.css",
                    "~/Content/site.css"));
        }
    }
}
