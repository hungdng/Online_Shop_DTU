using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string BillCode { get; set; }

        [MaxLength(50)]
        public string OrderStatusID { get; set; }

        [ForeignKey("OrderStatusID")]
        public virtual OrderStatus OrderStatus { get; set; }           

        public decimal TotalCost { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        [MaxLength(128)]
        [Column(TypeName = "nvarchar")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual AppUser User { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }

        [StringLength(256)]
        [Column(TypeName = "nvarchar")]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(256)]
        [Column(TypeName = "nvarchar")]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        [MaxLength(256)]
        public string PaymentMethod { set; get; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
