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
                "~/Scripts/jquery-1.10.2.min.js",
                "~/Scripts/DateUtil.js",
                        // jquery Ui jstree
                 "~/Scripts/jquery-ui.min.js",
                 "~/Scripts/lib/jstree/jstree.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      // bootstrap table
                      "~/Scripts/lib/bootstrap-table/bootstrap-table.js",
                      "~/Scripts/lib/bootstrap-table/bootstrap-table-locale-all*",

                      //  三个插件
                      "~/Scripts/lib/bootstrap-table/extensions/export/bootstrap-table-*",
                      "~/Scripts/lib/bootstrap-table/extensions/toolbar/bootstrap-table-*",
                      "~/Scripts/lib/bootstrap-table/extensions/filter-control/bootstrap-table-*",
                      "~/Scripts/lib/bootstrap-table/extensions/multiple-search/bootstrap-table-*",
                      "~/Scripts/lib/bootstrap-table/extensions/multiple-sort/bootstrap-table-multiple-sort.js",

                      // bootstrap datepicker
                      "~/Scripts/lib/datepicker/bootstrap-datepicker.js",
                      "~/Scripts/lib/datepicker/locales/bootstrap-datepicker.zh-CN.min.js",

                       // datejs
                       "~/Scripts/lib/datepicker/date-zh-CN.js",
                      "~/Scripts/lib/datepicker/globalization/zh-CN.js",

                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Scripts/lib/jstree/themes/default/style.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-table.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/base.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}