using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace QuickBootstrap
{
    public class Configuration
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