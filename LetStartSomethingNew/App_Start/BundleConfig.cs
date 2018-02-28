using System.Web;
using System.Web.Optimization;

namespace LetStartSomethingNew
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/css/site.css",
                        "~/css/jquery-ui.min.css",
                        "~/css/jquery-ui.theme.css",
                        "~/css/jquery-ui.structure.min.css",
                        "~/css/bootstrap.min.css",
                        "~/css/bootswatch.css"
              ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/js/jquery-1.10.2.min.js",
                        "~/js/callmyajax.js",
                        "~/js/callmyjs.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                         "~/js/jquery-ui.min.js",
                         "~/js/jquery.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/js/jquery.validate.min.js",
                        "~/js/jquery.unobtrusive-ajax.min.js",
                        "~/js/jquery.validate.unobstructive.min.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/js/bootstrap.min.js"
                      ));
                      //,
                      //"~/Scripts/respond.js"

        }
    }
}
