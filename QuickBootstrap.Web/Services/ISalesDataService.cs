using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using QuickBootstrap.Entities;
using QuickBootstrap.Models;
using QuickBootstrap.Services.Util;

namespace QuickBootstrap.Services
{
    public interface ISalesDataService
    {
        bool InsertSalesData(SalesData model);

        bool UpdateSalesData(Expression<Func<SalesData, bool>> whereExp, Action<SalesData> setValue, OrderData data);

        PagedList<SalesData> GetSalesData(QueryParams1  queryParams);

        PagedList<SalesReport> GetSalesReport(QueryParams2 queryParams);
    }
}
