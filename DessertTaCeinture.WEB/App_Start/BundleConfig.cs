using System.Web.Optimization;

namespace DessertTaCeinture.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.easing.1.3.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/jquery.waypoints.min.js",
                        "~/Scripts/owl.carousel.min.js",
                        "~/Scripts/jquery.countTo.js",
                        "~/Scripts/jquery.stellar.min.js",
                        "~/Scripts/jquery.magnific-popup.min.js",
                        "~/Scripts/magnific-popup-options.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"
                      ));

            bundles.Add(new StyleBundle("~/font/css").Include(
                        "~/Content/css/icomoon.css",
                        "~/Content/css/themify-icons.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                     "~/Content/css/animate.css",
                     "~/Content/css/bootstrap.css",
                     "~/Content/css/magnific-popup.css",
                     "~/Content/css/bootstrap-datetimepicker.min.css",
                     "~/Content/css/owl.carousel.min.css",
                     "~/Content/css/owl.theme.default.min.css",
                     "~/Content/css/style.css"
                     ));
        }
    }
}
