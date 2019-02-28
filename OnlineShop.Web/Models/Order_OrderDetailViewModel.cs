using MvcPaging;
using OnlineShop.Data.Models;
using OnlineShop.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class Order_OrderDetailViewModel
    {
        [Required]
        public OrderViewModel Orders { get; set; }
        public PaginationSet<OrderDetailViewModel> OrderDetails { get; set; }
    }
}