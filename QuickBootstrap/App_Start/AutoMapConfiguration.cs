using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace QuickBootstrap
{
    /// <summary>
    /// AutoMapper 对象映射配置
    /// </summary>
    public class AutoMapConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<Profiles.SaleDataRequestProfile>();
            });
        }
    }
}