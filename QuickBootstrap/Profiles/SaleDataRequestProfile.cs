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
                        DateTime.ParseExact((src.Yyyymmdd + src.Hhmiss), "yyyyMMddHHmmss", null)    // yyyyMMddHHmmss 是12 小时制
                        ));

            CreateMap<OrderData, SalesData>()
                .ForMember(dest => dest.GenerationTime, opt => opt.MapFrom(src => src.Order_time))
                .ForMember(dest => dest.O_cd, opt => opt.MapFrom(src => src.Order_code))
                .ForMember(dest => dest.M_id, opt => opt.MapFrom(src => src.Merchant_id))
                .ForMember(dest => dest.Comm, opt => opt.MapFrom(src => src.Commission))
                .ForMember(dest => dest.GenerationTime, opt => opt.MapFrom(src => DateTime.ParseExact(src.Order_time.Replace(" ",""), "yyyyMMddHHmmss", null)))
                .ForMember(dest => dest.P_cd, opt => opt.MapFrom(src => src.Product_code))
                .ForMember(dest => dest.It_cnt, opt => opt.MapFrom(src => src.Item_count))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Item_price)))
                .ForMember(dest => dest.C_cd, opt => opt.MapFrom(src => src.Category_code))


                .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src.Sales))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }


}