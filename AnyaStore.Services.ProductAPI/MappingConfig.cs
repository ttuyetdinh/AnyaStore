using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Models;
using AnyaStore.Services.ProductAPI.Models.DTO;
using AutoMapper;

namespace AnyaStore.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDTO>().ReverseMap();
                config.CreateMap<Category, CategoryDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}