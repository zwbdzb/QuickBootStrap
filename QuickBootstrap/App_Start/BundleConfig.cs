using System.Web;
using System.Web.Optimization;

namespace QuickBootstrap
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      // bootstrap table
                      "~/Scripts/bootstrap-table.js",
                      "~/Scripts/bootstrap-table-locale-all*",

                      //  三个插件
                      "~/Scripts/extensions/export/bootstrap-table-*",
                      "~/Scripts/extensions/toolbar/bootstrap-table-*",
                      "~/Scripts/extensions/filter-control/bootstrap-table-*",
                      "~/Scripts/extensions/multiple-search/bootstrap-table-*",

                      // bootstrap datetimepicker
                      "~/Scripts/js/bootstrap-datetimepicker.js",
                      "~/Scripts/js/locales/bootstrap-datetimepicker.zh-CN.js",

                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-table.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/base.css"));
        }
    }
}