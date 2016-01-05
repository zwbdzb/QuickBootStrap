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
            // 每天总共执行90次
            for(int i=1;i<=90;i++)
            {
                ExecQuery(i);
            }
        }

        // http://www.linktech.cn/AC/trans_list.htm?account_id=xiaoqi3535&sign=6e4ffd56aac70e0ebe8d670946624f58&syyyymmdd=20150901&eyyyymmdd=20150931&type=&affiliate_id=&merchant_id=&stat=true&output_type=json
        public void ExecQuery(int i)
        {   
            var sign = EncryptUtil.Encrypt(AccountId + "^" + AccountPwd);
               sign = "6e4ffd56aac70e0ebe8d670946624f58";
            var queryEndTime = DateTime.Now.AddDays(-30);
            var queryStartTime = queryEndTime.AddDays(-90);

            var req = new RestRequest("AC/trans_list.htm", Method.GET);
            req.AddQueryParameter("account_id", AccountId);
            req.AddQueryParameter("sign", sign);
            req.AddQueryParameter("syyyymmdd", queryStartTime.ToString("yyyyMMdd"));
            req.AddQueryParameter("eyyyymmdd", queryStartTime.AddDays(i).ToString("yyyyMMdd"));
            req.AddQueryParameter("type", "cps");
            req.AddQueryParameter("affiliate_id", "");            // 其余可选参数
            req.AddQueryParameter("merchant_id", "");
            req.AddQueryParameter("stat", "certain");
            req.AddQueryParameter("output_type", "");
            req.AddQueryParameter("sbill_yyyymmdd", "");
            req.AddQueryParameter("sbill_yyyymmdd", "");
            req.AddQueryParameter("u_id", "");
            req.AddQueryParameter("callback", "json");

            var resContent = WebClient.Execute(req).Content;
            var orderResp = JsonConvert.DeserializeObject<OrderResponse>(resContent);

            if (!string.IsNullOrEmpty(orderResp.Is_success) && orderResp.Is_success == "TRUE")
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
                            x.Stat_code = c.State_code;
                            x.Stat_desc = c.State_desc;
                            x.Cancel_comment = c.Cancel_comment;
                            x.Bill_yyyymmdd = DateTime.Parse(c.Bill_yyyymmdd);
                            x.UpdateTime = DateTime.Now;
                        });
                }
            }
            else
            {
                var log = LogManager.GetLogger(typeof(PerformanceExportJob));
                log.Error(string.Format("{0}-{1}-start query error", DateTime.Now,i));
            }
        }
    }
}