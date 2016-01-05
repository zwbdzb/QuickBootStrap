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


        // 查询当日起，往前查询120天到30 天的数据，现在不能确定是每天执行，还是间隔执行
        public void Execute(IJobExecutionContext context)
        {
            Debug.WriteLine(DateTime.Now+"作业开始执行");
            // 每天总共执行90次
            for(int i=0;i<=90;i++)
            {
                ExecQuery(i);
            }
        }

        public void ExecQuery(int i)
        {
            var sign = EncryptUtil.Encrypt(AccountId + "^" + AccountPwd);
            var queryEndTime = DateTime.Now.AddDays(-30);
            var queryStartTime = queryEndTime.AddDays(-90);

            var req = new RestRequest("AC/trans_list.htm", Method.GET);
            req.AddParameter("account_id", AccountId);
            req.AddParameter("sign", EncryptUtil.Encrypt(AccountId + "^" + AccountPwd));
            req.AddParameter("syyyymmdd", queryStartTime);
            req.AddParameter("eyyyymmdd", queryStartTime.AddDays(i));
            req.AddParameter("type", "cps");
            req.AddParameter("affiliate_id", "");            // 其余可选参数
            req.AddParameter("merchant_id", "");
            req.AddParameter("stat", "certain");
            req.AddParameter("output_type", "");
            req.AddParameter("sbill_yyyymmdd", "");
            req.AddParameter("sbill_yyyymmdd", "");
            req.AddParameter("u_id", "");
            req.AddParameter("callback", "json");

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