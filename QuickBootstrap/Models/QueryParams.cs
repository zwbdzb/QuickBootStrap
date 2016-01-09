using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBootstrap.Models
{


    public class QueryParams
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }                // 分类
        public string Order { get; set; }               // 排序

        public QueryParams()
        {
            Sort = "Yyyymmdd";
            Order = "desc";
        }
    }

    public  class QueryParams1:QueryParams
    {
        public int? STime { get; set; }         // 搜素开始时间
        public int? ETime { get; set; }         // 搜素开始时间

        public string M_id { get; set; }            // 广告主

        public string QueryType { get; set; }       // 查询类型

        public string TypeValue { get; set; }

        public int?  Stat { get; set; }
    }

    public class QueryParams2 : QueryParams
    {
        public int? STime { get; set; }         // 搜素开始时间
        public int? ETime { get; set; }         // 搜素开始时间

        public string M_id { get; set; }            // 广告主

        public string WebSite { get; set; }

        public int? Stat { get; set; }
    }

    public class RespResult<T>
    {
        public int IsOk { get; set; }
        public int Count { get; set; }
        public List<T> Result { get; set; }
    }
}
