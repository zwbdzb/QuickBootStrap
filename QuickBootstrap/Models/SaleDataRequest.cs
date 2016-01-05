using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBootstrap.Models
{
    public class SaleDataRequest
    {
        public  string Yyyymmdd { get; set; }
        public string Hhmiss { get; set; }

        public string O_cd { get; set; }
        public string M_id { get; set; }

        public string Mbr_id { get; set; }

        public string Comm { get; set; }
        public string U_id { get; set; }

        public string P_cd { get; set; }

        public string It_cnt { get; set; }

        public string Price { get; set; }

        public string C_cd { get; set; }
    }
}
