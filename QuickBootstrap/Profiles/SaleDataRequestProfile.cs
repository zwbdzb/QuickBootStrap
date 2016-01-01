using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using QuickBootstrap.Entities;
using QuickBootstrap.Models;

namespace QuickBootstrap.Profiles
{
    // 命名的映射类
    public class SaleDataRequestProfile: Profile
    {
        public SaleDataRequestProfile()
            : base("SaleDataRequestProfile")
        {
            
        }

        protected  override void Configure()
        {
            CreateMap<SaleDataRequest, SalesData>()
                .ForMember(dest => dest.AddTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.GenerationTime, opt =>
                    opt.MapFrom(src =>
                        DateTime.ParseExact((src.Yyyymmdd + src.Hhmiss), "yyyyMMddhhmmss", null)
                        ));

        }
    }


}