using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QuickBootstrap.Mvc;

namespace QuickBootstrap.Controllers.WebAPI
{
    [RoutePrefix("api/sales")]
    public class SalesDataController:JsonApiController
    {
        [HttpGet]
        public IHttpActionResult SalesData()
        {
            return null;
        }

    }
}