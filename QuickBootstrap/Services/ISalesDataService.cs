using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickBootstrap.Entities;

namespace QuickBootstrap.Services
{
    public interface ISalesDataService
    {
        bool InsertSalesData(SalesData model);

        bool UpdateSalesData(Func<SalesData, bool> whereExp, Action<SalesData> setValue);
    }
}
