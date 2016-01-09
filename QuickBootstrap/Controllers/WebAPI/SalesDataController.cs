using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Common.Logging;
using log4net.DateFormatter;
using Microsoft.Practices.Unity;
using QuickBootstrap.Entities;
using QuickBootstrap.Extendsions;
using QuickBootstrap.Helpers;
using QuickBootstrap.Models;
using QuickBootstrap.Mvc;
using QuickBootstrap.Services;
using QuickBootstrap.Validations;

namespace QuickBootstrap.Controllers.WebAPI
{
    [RoutePrefix("api/sales")]
    public class SalesDataController:JsonApiController
    {            
        private readonly ISalesDataService _salesDataService = UnityHelper.Instance.Unity.Resolve<ISalesDataService>();

        [HttpGet]
        [Route("", Name = "SalesData")]
        public IHttpActionResult SalesData([FromUri]SaleDataRequest request)
        { 
            var model = Mapper.Map<SalesData>(request);
            if (!_salesDataService.InsertSalesData(model))
            {
                return Json(new  ResponseError { Field="Ex",Msg = "Server internal exception." });
            }
            return Json(new {success = true});

        }

        [HttpGet]
        [Route("~/api/data/SalesData", Name = "GetSalesData")]
        public IHttpActionResult GetSalesData([FromUri]QueryParams1 queryParams)
        {
            var  data = _salesDataService.GetSalesData(queryParams);
            return Json(new RespResult<SalesData>
            {
                IsOk=1,
                Count = data.TotalCount,
                Result = data
            });
        }


        [HttpGet]
        [Route("~/api/data/SalesReport", Name = "GetSalesReport")]
        public IHttpActionResult GetSalesReport([FromUri]QueryParams2 queryParams)
        {
            var data = _salesDataService.GetSalesReport(queryParams);
            return Json(new RespResult<SalesReport>
            {
                IsOk = 1,
                Count = data.TotalCount,
                Result = data
            });
        }


    }
}