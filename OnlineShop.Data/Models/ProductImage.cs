using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        public Guid ProductImageID { get; set; }

        public Guid ProductID { get; set; }

        [MaxLength(250)]
        public string Path { get; set; }

        [MaxLength(256)]
        public string Caption { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
