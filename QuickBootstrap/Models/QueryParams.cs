using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBootstrap.Models
{
    public  class QueryParams
    {
        public  int PageSize { get; set; }
        public  string  UserName { get; set; }
    }

    public class RespResult<T>
    {
        public int IsOk { get; set; }
        public int Count { get; set; }
        public List<T> Result { get; set; }
    }
}
