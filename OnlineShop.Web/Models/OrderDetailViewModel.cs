using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace OnlineShop.Web.Models
{
    public class OrderDetailViewModel
    {
        public Guid OrderDetailID { get; set; }

        [Required]
        [Display(Name = "Order ID")]
        [MaxLength(256)]
        public Guid OrderID { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        [MaxLength(256)]
        public Guid ProductID { get; set; }

        [Required]
        [Display(Name = "Sale Number")]
        [Range(1, 100, ErrorMessage = "Sale Number has to be OVER 1")]
        public decimal SaleNumber { get; set; }

        [Required]
        [Display(Name = "Sale Price")]
        public decimal SalePrice { get; set; }

        [Required]
        [Display(Name = "Total")]
        public decimal TotalCost { get; set; }

        [MaxLength(500)]
        public string ProductSpec { get; set; }

        public virtual OrderViewModel Order { get; set; }

        public virtual ProductViewModel Product { get; set; }
    }
}