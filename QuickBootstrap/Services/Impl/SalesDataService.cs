using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using QuickBootstrap.Entities;
using QuickBootstrap.Extendsions;
using QuickBootstrap.Models;
using QuickBootstrap.Services.Util;
using System.Linq.Dynamic;

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

        /// <summary>
        /// 更新销售数据，这里根据订单更新
        /// </summary>
        /// <param name="whereExp"> 这里必须使用表达式树 才能使用 IQueryable的延迟执行的特性 </param>
        /// <param name="setValue"> 更新数据</param>
        /// <param name="data"> 添加数据 </param>
        /// <returns></returns>
        public bool UpdateSalesData(Expression<Func<SalesData, bool>> whereExp, Action<SalesData> setValue, OrderData data)
        {
            var model = DbContext.SalesData.SingleOrDefault(whereExp);
            try
            {
                if (model == null)
                    return InsertSalesData(Mapper.Map<SalesData>(data));
                setValue(model);
                DbContext.Entry(model).State = EntityState.Modified;
                return DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // 必须动态生成查询字符串
        public PagedList<SalesData> GetSalesData(QueryParams1 queryParams)
        {
            var  data= DbContext.SalesData.AsQueryable();
            if (queryParams.STime.HasValue && queryParams.ETime.HasValue )
            {
                if (queryParams.STime.Value != queryParams.ETime.Value)
                    // data = data.Where(x => x.Yyyymmdd >= queryParams.STime && x.Yyyymmdd<= queryParams.ETime );
                    data = data.Where("Yyyymmdd>=" + queryParams.STime + " and Yyyymmdd<=" +queryParams.ETime);
                else
                    data = data.Where(x => x.Yyyymmdd == queryParams.STime);
            }
            if (queryParams.Stat.HasValue)
            {
                data = data.Where(x => x.Stat_code == queryParams.Stat);
            }

            if (!string.IsNullOrEmpty(queryParams.M_id))
            {
                data = data.Where(x => x.M_id == queryParams.M_id);
            }
            // 以下便是动态linq：查询字段动态
            if (!string.IsNullOrEmpty(queryParams.TypeValue))
            {
                data = data.Where(queryParams.QueryType + "=" + queryParams.TypeValue);

                //if (queryParams.QueryType == "O_cd")
                //   data = data.Where(x => x.O_cd == queryParams.TypeValue);
                //else
                //    data = data.Where(x => x.P_cd == queryParams.TypeValue);
            }
            var query = data.OrderBy(o => o.Yyyymmdd).ThenBy(o => o.Hhmiss).ToPagedList(queryParams.Offset/queryParams.Limit+1, queryParams.Limit);

            return query;
        }




        public PagedList<SalesReport> GetSalesReport(QueryParams2 queryParams)
        {
            var  data= DbContext.SalesData.AsQueryable();
            if (queryParams.STime.HasValue && queryParams.ETime.HasValue )
            {
                if (queryParams.STime.Value != queryParams.ETime.Value)
                    data = data.Where(x => x.Yyyymmdd >= queryParams.STime && x.Yyyymmdd<= queryParams.ETime );
                else
                    data = data.Where(x => x.Yyyymmdd == queryParams.STime );
            }
            if (!string.IsNullOrEmpty(queryParams.M_id))
            {
                data = data.Where(x => x.M_id == queryParams.M_id);
            }

            if (!string.IsNullOrEmpty(queryParams.WebSite))
            {
                data = data.Where(x => x.Affiliate_id == queryParams.WebSite);
            }

            var mydataGrouped = data.GroupBy(queryParams.Sort, "it").Select("new (it.Key as Sort,Count() as OrderCount,SUM(Sales) as SalesTotal, SUM(Comm) as CommTotal,it as Report )");

            List<SalesReport> reports = new List<SalesReport>();

            foreach (dynamic group in mydataGrouped)
            {
                int  _validOrderCount = 0;
                decimal _validSales = 0;
                decimal _validComm=0;
                foreach (dynamic ii  in group.Report)
                {
                    if (ii.Stat_code == 200)
                    {
                        _validOrderCount += 1;
                        _validSales += ii.Sales;
                        _validComm += ii.Commission;
                    }
                }

                reports.Add(new SalesReport
                {
                    Sort = group.Sort,
                    OrderCount = group.OrderCount,
                    ValidOrderCount =_validOrderCount,
                    SalesTotal = group.SalesTotal,
                    ValidSalesTotal = _validSales, 
                    CommTotal = group.CommTotal,
                    ValidCommTotal =  _validComm
                });
            }

            return reports.AsQueryable().OrderByDescending(x => x.Sort).ToPagedList(queryParams.Offset / queryParams.Limit + 1, queryParams.Limit);
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