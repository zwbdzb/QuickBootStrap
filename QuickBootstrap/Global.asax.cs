using System;
using System.Configuration;
using System.Data.Entity;
using System.Runtime.Remoting.Channels;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using Quartz;
using Quartz.Impl;
using QuickBootstrap.App_Start;
using QuickBootstrap.Entities;

namespace QuickBootstrap
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 在托管代码抛出异常的时候，将异常信息使用Log4J 管理起来
            //AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            //{
            //    var log = LogManager.GetLogger(typeof(MvcApplication));
            //    log.Error(args.Exception.Message);
            //};


            // 注册区域
            AreaRegistration.RegisterAllAreas();
            // Web API 启动特性路由
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // 注册全局过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // 注册网站路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutoMapConfiguration.Configure();
            // 注册js，css压缩
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JobScheduler.Start();

            InitDataBase();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            JobScheduler.Stop();

            var log = LogManager.GetLogger(typeof(MvcApplication));
            log.Info(string.Format("{0}- website applicetion end ", DateTime.Now ));

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            JobScheduler.Stop();

            var lastError = Server.GetLastError().GetBaseException();
            var log = LogManager.GetLogger(typeof(MvcApplication));
            log.Error(lastError);

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
