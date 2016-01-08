using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using QuickBootstrap.Entities;
using QuickBootstrap.Extendsions;
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

        // 必须动态生成查询字符串
        public PagedList<SalesData> GetSalesData(QueryParams queryParams)
        {
            var  data= DbContext.SalesData.AsQueryable();
            if (!String.IsNullOrEmpty(queryParams.SpecTime))
            {
                data = data.Where(x => x.Yyyymmdd.Equals(queryParams.SpecTime,StringComparison.InvariantCultureIgnoreCase));
            }
            var query = data.OrderBy(o => o.Yyyymmdd).ThenBy(o => o.Hhmiss).ToPagedList(queryParams.Offset/queryParams.Limit+1, queryParams.Limit);

            return query;
        }


        /// <summary>
        /// Pages the specified query.
        /// </summary>
        /// <typeparam name="T">Generic Type Object</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The Object query where paging needs to be applied.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
        /// <param name="rowsCount">The total rows count.</param>
        /// <returns></returns>
        private static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }

    }
}