using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        [Key]
        [MaxLength(50)]
        public string OrderStatusID { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName = "nvarchar")]
        public string OrderStatusName { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "nvarchar")]
        public string Description { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
