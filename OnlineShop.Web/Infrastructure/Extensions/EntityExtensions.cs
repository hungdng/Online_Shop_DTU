using OnlineShop.Common.Helpers;
using OnlineShop.Data.Models;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryViewModel)
        {
            productCategory.ID = productCategoryViewModel.ID;
            productCategory.Name = productCategoryViewModel.Name;
            productCategory.Alias = StringHelper.ToUnsignString(productCategoryViewModel.Name);

            productCategory.Description = productCategoryViewModel.Description;
            productCategory.DisplayOrder = productCategoryViewModel.DisplayOrder;
            productCategory.ParentID = productCategoryViewModel.ParentID;

            productCategory.CreatedDate = productCategoryViewModel.CreatedDate;
            productCategory.CreatedBy = productCategoryViewModel.CreatedBy;
            productCategory.UpdatedDate = productCategoryViewModel.UpdatedDate;
            productCategory.UpdatedBy = productCategoryViewModel.UpdatedBy;
            productCategory.Status = productCategoryViewModel.Status;
        }

        public static void UpdateAppRole(this AppRole appRole, AppRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.IsDeleted = appRoleViewModel.IsDeleted;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this AppUser appUser, AppUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.Email;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Gender = appUserViewModel.Gender;
            appUser.IsDeleted = appUserViewModel.IsDeleted;
            appUser.CreatedDate = appUserViewModel.CreatedDate;
            appUser.UpdatedDate = appUserViewModel.UpdatedDate;

        }

        public static void UpdateManufacture(this Manufacture manufacture, ManufactureViewModel manufactureViewModel)
        {
            manufacture.ManufactureID = manufactureViewModel.ManufactureID;
            manufacture.ManufactureName = manufactureViewModel.ManufactureName;

            manufacture.Description = manufactureViewModel.Description;
            manufacture.Logo = manufactureViewModel.Logo;
            manufacture.Url = manufactureViewModel.Url;
        }

        public static void UpdateOrder(this Order order, OrderViewModel orderViewModel, string action = "add")
        {
            if (action == "update")
            {
                order.ID = orderViewModel.ID;
                order.UpdatedDate = DateTime.Now;
            }                
            else if(action == "add")
            {
                order.ID = Guid.NewGuid();
                order.BillCode = StringHelper.RandomString("HD".ToString(), 10);
            }
                
            order.TotalCost = orderViewModel.TotalCost;
            order.Amount = orderViewModel.Amount;
            order.Discount = orderViewModel.Discount;
            order.CustomerName = orderViewModel.CustomerName;
            order.CustomerAddress = orderViewModel.CustomerAddress;
            order.CustomerEmail = orderViewModel.CustomerEmail;
            order.CustomerMobile = orderViewModel.CustomerMobile;
            order.Note = orderViewModel.Note;
            order.PaymentMethod = orderViewModel.PaymentMethod;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = orderViewModel.CreatedBy;
            order.OrderStatusID = orderViewModel.OrderStatusID;
            order.CustomerId = orderViewModel.CustomerId;
        }

        public static void UpdateOrderDetail(this OrderDetail orderDetail, OrderDetailViewModel orderDetailViewModel, string action = "add")
        {
            if (action == "update")
            {
                orderDetail.OrderDetailID = orderDetailViewModel.OrderDetailID;
            }
            else if (action == "add")
            {
                orderDetail.OrderDetailID = Guid.NewGuid();
            }

            orderDetail.OrderID = orderDetailViewModel.OrderID;
            orderDetail.ProductID = orderDetailViewModel.ProductID;
            orderDetail.SaleNumber = orderDetailViewModel.SaleNumber;
            orderDetail.SalePrice = orderDetailViewModel.SalePrice;
            orderDetail.TotalCost = orderDetailViewModel.TotalCost;
            orderDetail.ProductSpec = orderDetailViewModel.ProductSpec;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productViewModel, string action = "add")
        {
            if (action == "update")
            {
                //product.ProductID = productViewModel.ProductID;
                product.UpdatedDate = DateTime.Now;
            }
            else
            {
                product.ProductID = Guid.NewGuid();
                product.CreatedDate = DateTime.Now;
            }
            
            product.ProductName = productViewModel.ProductName;
            product.Alias = StringHelper.ToUnsignString(productViewModel.ProductName);

            product.Description = productViewModel.Description;
            product.ProductCategoryID = productViewModel.ProductCategoryID;
            product.ManufactureID = productViewModel.ManufactureID;
            product.ThumbnailImage = productViewModel.ThumbnailImage;                        
            product.HotFlag = productViewModel.HotFlag;
            product.Price = productViewModel.Price;
            product.OriginalPrice = productViewModel.OriginalPrice;
            product.PromotionPrice = productViewModel.PromotionPrice != null ? productViewModel.PromotionPrice : 0;
            product.Warranty = productViewModel.Warranty;
            product.FullDescription = productViewModel.FullDescription;
            product.ViewCount = productViewModel.ViewCount != null ? productViewModel.ViewCount : 0;
            product.Tags = productViewModel.Tags;
            product.Quantity = productViewModel.Quantity;
            product.CalculationUnit = productViewModel.CalculationUnit;
            
            product.CreatedBy = productViewModel.CreatedBy;
            product.UpdatedBy = productViewModel.UpdatedBy;           
            product.IsBuy = productViewModel.IsBuy;
        }
    }
}