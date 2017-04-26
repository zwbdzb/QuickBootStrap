using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using QuickBootstrap.Attributes;

namespace QuickBootstrap.Models
{
    //网站配置
    public class WebSiteConfig
    {
        [BootstrapTextBox]
        [DisplayName("领克连接账户")]
        public string LinktechAccountId { get; set; }

        [BootstrapHidden]
        public string LinktechAccountPwd { get; set; }

        [BootstrapTextBox]
        [DisplayName("接收领克数据API地址")]
        public string LinktechReceiveApiUrl { get; set; }

        [BootstrapTextBox]
        [DisplayName("存储数据库")]
        public string Database { get; set; }

    }
}