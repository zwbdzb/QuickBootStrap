using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using QuickBootstrap.Entities;

namespace QuickBootstrap
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 注册区域
            AreaRegistration.RegisterAllAreas();
            // Web API 启动特性路由
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // 注册全局过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // 注册网站路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 注册js，css压缩
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitDataBase();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError().GetBaseException();
            {
                var log = LogManager.GetLogger(typeof(MvcApplication));
                log.Error(lastError);
            }
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private static void InitDataBase()
        {
            //数据库不存在时创建
            Database.SetInitializer(new CreateDatabaseIfNotExists<DefaultDbContext>());

            //初始化数据
            Database.SetInitializer(new InitData());

            var log = LogManager.GetLogger(typeof(MvcApplication));
            log.Info(string.Format("{0}-start", DateTime.Now));
        }
    }
}
