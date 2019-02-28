using OnlineShop.Data.Models;
using OnlineShop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Infrastructure.Extensions
{
    public static class CategoryExtensions
    {
        public static string GetFormattedBreadCrumb(this ProductCategory category,
           IList<ProductCategory> allCategories,
           IProductCategoryService categoryService,
           string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;
            
            var breadcrumb = GetCategoryBreadCrumb(category, allCategories, true);            
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = String.IsNullOrEmpty(result)
                    ? categoryName
                    : string.Format("{0} {1} {2}", result, separator, categoryName);
            }

            return result;
        }

        public static IList<ProductCategory> GetCategoryBreadCrumb(this ProductCategory category,
            IList<ProductCategory> allCategories, bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<ProductCategory>();

            //used to prevent circular references
            var alreadyProcessedCategoryIds = new List<Guid>();

            while (category != null && //not null
                showHidden &&
                !alreadyProcessedCategoryIds.Contains(category.ID)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.ID);

                category = (from c in allCategories
                            where c.ID == category.ParentID
                            select c).FirstOrDefault();
            }
            result.Reverse();
            return result;
        }
    }
}