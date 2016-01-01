using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using QuickBootstrap.Entities;
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
           
           // _salesDataService.InsertSalesData(model);
            return Json(new {success = true});
        }

    }
}