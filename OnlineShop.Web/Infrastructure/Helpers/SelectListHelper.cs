using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Infrastructure.Helpers
{
    public class SelectListHelper
    {
        public static List<SelectListItem> GetCategoryList(IProductCategoryService categoryService)
        {
            var result = new List<SelectListItem>();
            var categories = categoryService.GetParentByDisplayOrder();

            foreach (var item in categories)
            {
                var itemFormattedBreadCrumb = GetFormattedBreadCrumb(item, categoryService);
                foreach (var xitem in itemFormattedBreadCrumb)
                {
                    var selectItem = new SelectListItem()
                    {
                        Text = xitem.Name,
                        Value = xitem.ID.ToString()
                    };
                    result.Add(selectItem);
                }
            }
            return result;
        }

        public static IList<ProductCategory> GetFormattedBreadCrumb(ProductCategory category,
           IProductCategoryService categoryService,
           string separator = ">>", int languageId = 0)
        {
            var result = new List<ProductCategory>();
            var list = categoryService.GetProductCategoryByParentID(category.ID);
            int count = list.Count();
            foreach (var jitem in list)
            {
                result.Add(new ProductCategory
                {
                    ID = jitem.ID,
                    Name = string.Format("{0} {1} {2}", category.Name, separator, jitem.Name)
                });
            }

            return result;
        }
    }
}