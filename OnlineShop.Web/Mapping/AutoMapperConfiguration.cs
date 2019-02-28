using AutoMapper;
using OnlineShop.Data.Models;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Manufacture, ManufactureViewModel>();
                x.CreateMap<ProductCategory, ProductCategoryViewModel>();
                x.CreateMap<Tag, TagViewModel>();
                x.CreateMap<Product, ProductViewModel>();
                x.CreateMap<ProductTag, ProductTagViewModel>();
                x.CreateMap<Order, OrderViewModel>();
                x.CreateMap<OrderDetail, OrderDetailViewModel>();
                x.CreateMap<AppRole, AppRoleViewModel>();
                x.CreateMap<AppUser, AppUserViewModel>();
            });
        }
    }
}