﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public Guid ProductID { get; set; }
        
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}