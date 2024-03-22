using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Models;
using AnyaStore.Services.ShoppingCartAPI.Models.DTO;
using AutoMapper;

namespace AnyaStore.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsUpsertDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}