using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
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
            _salesDataService.InsertSalesData(model);
            return Json(new {success = true});
        }

    }
}