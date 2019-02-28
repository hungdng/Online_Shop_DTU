using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace OnlineShop.Web.Models
{
    public class OrderViewModel
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Bill Code")]
        [MaxLength(256)]
        public string BillCode { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please choose at least one status")]
        [BindRequired]
        public string OrderStatusID { get; set; }

        [Display(Name = "Total Payments")]
        public decimal TotalCost { get; set; }

        [Display(Name = "Payments Before Discount")]
        public decimal Amount { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [MaxLength(128)]
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        [MaxLength(256)]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        [Display(Name = "Contact")]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }

        [StringLength(256)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(256)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        [MaxLength(256)]
        [Display(Name = "Payment Method")]
        [Required(ErrorMessage = "Please choose at least one payment method")]
        public string PaymentMethod { set; get; }

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }

        public virtual OrderStatusViewModel OrderStatus { get; set; }
    }
}