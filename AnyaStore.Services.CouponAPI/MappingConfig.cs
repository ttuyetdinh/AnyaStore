using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Models;
using AnyaStore.Services.CouponAPI.Models.DTO;
using AutoMapper;

namespace AnyaStore.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}