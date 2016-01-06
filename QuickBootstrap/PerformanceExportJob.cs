using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Common.Logging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using QuickBootstrap.Cache;
using QuickBootstrap.Helpers;
using QuickBootstrap.Models;
using QuickBootstrap.Services;
using QuickBootstrap.Services.Util;
using RestSharp;

namespace QuickBootstrap
{
    public class PerformanceExportJob:IJob
    {
        ILog log = LogManager.GetLogger(typeof(PerformanceExportJob));

        private readonly ISalesDataService _salesDataService = UnityHelper.Instance.Unity.Resolve<ISalesDataService>();

        private static RestClient _client;
        private static RestClient WebClient
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient("http://www.linktech.cn");
                }
                return _client;
            }
        }

        private static string AccountId
        {
            get { return ConfigurationManager.AppSettings["linktechAccountId"]; }
        }

        private static string AccountPwd
        {
            get { return ConfigurationManager.AppSettings["linktechAccountPwd"]; }
        }


        // 每天执行90次，每次分别执行 -120 天到-90 天的业绩数据
        public void Execute(IJobExecutionContext context)
        {
            Debug.WriteLine(DateTime.Now+"作业开始执行");
            // 作业范围
            var queryStartTime = DateTime.Now.AddDays(-30).AddDays(-90);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i=1;i<=90;i++)
            {
                ExecQuery(queryStartTime);
                queryStartTime = queryStartTime.AddDays(1);
            }
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            log.Info(string.Format("{0}-start query  cost time {1} ", DateTime.Now, time));
        }

        // http://www.linktech.cn/AC/trans_list.htm?account_id=xiaoqi3535&sign=6e4ffd56aac70e0ebe8d670946624f58&syyyymmdd=20150901&eyyyymmdd=20150931&type=&affiliate_id=&merchant_id=&stat=true&output_type=json
        public void ExecQuery(DateTime startTime)
        {
            var sign = EncryptUtil.MD5ForPHP(AccountId + "^." + AccountPwd);    // 
            sign = "6e4ffd56aac70e0ebe8d670946624f58";
            var req = new RestRequest("/AC/trans_list.htm");
            req.AddQueryParameter("account_id", AccountId);
            req.AddQueryParameter("sign", sign);
            req.AddQueryParameter("syyyymmdd", startTime.ToString("yyyyMMdd"));
            req.AddQueryParameter("eyyyymmdd", startTime.AddDays(1).ToString("yyyyMMdd"));
            req.AddQueryParameter("type", "cps");
            req.AddQueryParameter("affiliate_id", "");            // 其余可选参数
            req.AddQueryParameter("merchant_id", "");
            req.AddQueryParameter("stat", "certain");
            req.AddQueryParameter("output_type", "json");
            //req.AddQueryParameter("sbill_yyyymmdd", "");
            //req.AddQueryParameter("sbill_yyyymmdd", "");
            //req.AddQueryParameter("u_id", "");
            //req.AddQueryParameter("callback", "CbFunction");

            var resContent = WebClient.Execute(req).Content;
            var orderResp = JsonConvert.DeserializeObject<JsonOrderResponse>(resContent);

            if (!string.IsNullOrEmpty(orderResp.Is_success) && orderResp.Is_success == "TRUE")
            {
                if (orderResp.List_count > 0 || orderResp.Order_list != null )
                {
                    //  根据订单号更新数据
                    foreach (var c in orderResp.Order_list)
                    {
                        _salesDataService.UpdateSalesData(
                            x => x.O_cd.Equals(c.Order_code, StringComparison.CurrentCultureIgnoreCase),
                            x =>
                            {
                                x.Sales = c.Sales;
                                x.Commission = c.Commission;
                                x.Stat_code = c.Stat_code;
                                x.Stat_desc = c.Stat_desc;
                                x.Cancel_comment = c.Cancel_comment;
                                x.Bill_yyyymmdd = c.Bill_yyyymmdd;
                                x.UpdateTime = DateTime.Now;
                            },c);
                    }
                }
                else
                {
                    log.Info(string.Format("{0}查询{1}-start query result : no data ", DateTime.Now, startTime));
                }
               
            }
            else 
            {

                log.Error(string.Format("{0}查询{1}-start query error:{3}", DateTime.Now, startTime ));
            }
        }
    }
}