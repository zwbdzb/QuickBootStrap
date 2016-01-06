using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickBootstrap.Models
{
    public class JsonOrderResponse
    {
        public string Is_success { get; set; }
        public int  List_count { get; set; }
        public List<OrderData> Order_list { get; set; }
    }

    public class OrderData
    {
        public string Order_type { get; set; }
        public string Order_time { get; set; }
        public string Merchant_id { get; set; }
        public string U_id { get; set; }
        public string Order_code { get; set; }
        public string Product_code { get; set; }

        public string Category_code { get; set; }
        public string Item_count { get; set; }
        public string Item_price { get; set; }
        public decimal Sales { get; set; }
        public decimal Commission { get; set; }

        public int  Stat_code { get; set; }
        public string Stat_desc { get; set; }

        public string Cancel_comment { get; set; }

        public string Bill_yyyymmdd { get; set; }
    }
}