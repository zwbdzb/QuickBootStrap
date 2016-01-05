using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using AutoMapper;
using QuickBootstrap.Entities;
using QuickBootstrap.Models;
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

        //  更新数据，这里是根据订单更新数据
        public bool UpdateSalesData(Func<SalesData, bool> whereExp, Action<SalesData> setValue, OrderData data)
        {
            var model = DbContext.SalesData.SingleOrDefault(whereExp);
            try
            {
                if (model != null)
                {
                    setValue(model);
                    DbContext.Entry(model).State = EntityState.Modified;
                    return DbContext.SaveChanges() > 0;
                }
                else
                {
                    var saleModel = Mapper.Map<SalesData>(data);
                    return  InsertSalesData(saleModel);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}