using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class MenuViewModel
    {
        public IEnumerable<ProductCategoryViewModel> ProductCategory { get; set; }
    }
}