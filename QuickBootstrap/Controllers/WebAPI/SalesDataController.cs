using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using QuickBootstrap.Entities;
using QuickBootstrap.Helpers;
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
        public IHttpActionResult SalesData(string yyyymmdd,string hhmiss,string o_cd,string m_id, string mbr_id, string comm,string u_id,string p_cd,string it_cnt,string price,string c_cd)
        {
            var model = new SalesData
            {
                Yyyymmdd = yyyymmdd,
                Hhmiss = hhmiss,
                O_cd = o_cd,
                M_id = m_id,
                Mbr_id = mbr_id,
                Comm = Decimal.Parse(comm),
                U_id = u_id,
                P_cd = p_cd,
                It_cnt = int.Parse(it_cnt),
                Price = Decimal.Parse(price),
                C_cd = c_cd,
                AddTime = DateTime.Now
            };
            _salesDataService.InsertSalesData(model);
            return null;
        }

    }
}