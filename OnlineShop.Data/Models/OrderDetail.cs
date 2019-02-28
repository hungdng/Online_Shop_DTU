using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }

        public Guid ProductID { get; set; }

        public decimal SaleNumber { get; set; }

        public decimal SalePrice { get; set; }

        public decimal TotalCost { get; set; }

        [MaxLength(500)]
        public string ProductSpec { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Orders { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
