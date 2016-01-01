using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using QuickBootstrap.Entities;
using QuickBootstrap.Services.Util;

namespace QuickBootstrap.Services.Impl
{
    public class SalesDataService:ServiceContext,ISalesDataService
    {
        public bool InsertSalesData(SalesData model)
        {
            try
            {
                DbContext.SalesData.Add(model);
                return DbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.ToList()[0].ValidationErrors;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}